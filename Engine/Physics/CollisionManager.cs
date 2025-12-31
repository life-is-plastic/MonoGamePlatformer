using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Engine.Core;
using Engine.Util;
using Engine.Util.Collections;
using Engine.Util.Extensions;
using Microsoft.Xna.Framework;

namespace Engine.Physics;

public partial class CollisionManager : Component
{
    internal InlineArray8<IndexedSet<Collider>> _layerToColliders;
    private readonly Dictionary<Entity, IndexedSet<ICollisionHandler>> _handlers = new();
    private Dictionary<(Collider, Collider), ContactInfo> _contacts = new();
    private Dictionary<(Collider, Collider), ContactInfo> _previousContacts = new();

    public CollisionLayers Layers { get; } = new();

    public CollisionManager()
    {
        for (var layer = 0; layer < _layerToColliders.Length; layer++)
        {
            _layerToColliders[layer] = new IndexedSet<Collider>();
        }
    }

    private void CheckContacts()
    {
        (_contacts, _previousContacts) = (_previousContacts, _contacts);
        _contacts.Clear();

        // Check same layer collisions.
        for (var layer = 0; layer < _layerToColliders.Length; layer++)
        {
            if (!Layers.IsCollidable(layer, layer))
            {
                continue;
            }
            var colliders = _layerToColliders[layer];
            for (var i = 0; i < colliders.Count - 1; i++)
            {
                for (var j = i + 1; j < colliders.Count; j++)
                {
                    CheckContactBetween(colliders[i], colliders[j]);
                }
            }
        }

        // Check cross-layer collisions.
        for (var l1 = 0; l1 < _layerToColliders.Length - 1; l1++)
        {
            for (var l2 = l1 + 1; l2 < _layerToColliders.Length; l2++)
            {
                if (!Layers.IsCollidable(l1, l2))
                {
                    continue;
                }
                foreach (var c1 in _layerToColliders[l1])
                {
                    foreach (var c2 in _layerToColliders[l2])
                    {
                        CheckContactBetween(c1, c2);
                    }
                }
            }
        }
    }

    private void CheckContactBetween(Collider a, Collider b)
    {
        if (a.Entity == b.Entity || !a.IsEnabled || !b.IsEnabled)
        {
            return;
        }
        if (a.Entity.Id > b.Entity.Id)
        {
            (a, b) = (b, a);
        }

        var minkowskiDiff = a.AsWorldRectangleF().GetMinkowskiDifference(b.AsWorldRectangleF());
        if (!minkowskiDiff.Contains(Vector2.Zero))
        {
            return;
        }

        _contacts.Add(
            (a, b),
            new ContactInfo()
            {
                Mine = a,
                Other = b,
                MinkowskiDifference = minkowskiDiff,
                Penetration = minkowskiDiff.GetMinkowskiPenetrationVector(),
            }
        );
    }

    private void HandleCollisions()
    {
        foreach (var ((a, b), prevContact) in _previousContacts)
        {
            if (!_contacts.ContainsKey((a, b)))
            {
                foreach (var handler in GetHandlers(a.Entity))
                {
                    handler.OnCollisionExit(prevContact);
                }
                foreach (var handler in GetHandlers(b.Entity))
                {
                    handler.OnCollisionExit(prevContact.Inverted());
                }
            }
        }

        foreach (var ((a, b), contact) in _contacts)
        {
            if (_previousContacts.ContainsKey((a, b)))
            {
                foreach (var handler in GetHandlers(a.Entity))
                {
                    handler.OnCollisionStay(contact);
                }
                foreach (var handler in GetHandlers(b.Entity))
                {
                    handler.OnCollisionStay(contact.Inverted());
                }
            }
            else
            {
                foreach (var handler in GetHandlers(a.Entity))
                {
                    handler.OnCollisionEnter(contact);
                }
                foreach (var handler in GetHandlers(b.Entity))
                {
                    handler.OnCollisionEnter(contact.Inverted());
                }
            }
        }
    }

    /// <summary>
    /// Convenience method that returns an empty set view if there are no handlers for the given
    /// entity.
    /// </summary>
    private IndexedSetView<ICollisionHandler> GetHandlers(Entity entity)
    {
        if (_handlers.TryGetValue(entity, out var handlers))
        {
            return new(handlers);
        }
        return new();
    }
}

public partial class CollisionManager : IUpdatable
{
    int IUpdatable.UpdateOrder => IUpdatable.UpdateOrderPhysics[^1];

    void IUpdatable.Update()
    {
        CheckContacts();
        HandleCollisions();
    }
}

public partial class CollisionManager : IEntitySyncer
{
    void IEntitySyncer.Sync(EntityChangelist entityChangelist)
    {
        foreach (var component in entityChangelist.Detached.Values)
        {
            if (component is Collider collider)
            {
                _layerToColliders[collider.Layer].RemoveOrDie(collider);
            }
            if (component is ICollisionHandler handler)
            {
                _handlers[handler.Entity].RemoveOrDie(handler);
            }
        }
        foreach (var entity in entityChangelist.Destroyed)
        {
            _handlers.Remove(entity);
        }

        foreach (var component in entityChangelist.Attached.Values)
        {
            if (component is Collider collider)
            {
                _layerToColliders[collider.Layer].AddOrDie(collider);
            }
            if (component is ICollisionHandler handler)
            {
                _handlers.GetOrAddNew(handler.Entity).AddOrDie(handler);
            }
        }
    }
}
