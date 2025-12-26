using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Engine.Core;
using Engine.Util;
using Engine.Util.Collections;
using Engine.Util.Extensions;
using Microsoft.Xna.Framework;

namespace Engine.Physics;

public partial class CollisionManager : Component
{
    private readonly List<(int, int)> _collidableLayers = new();
    private readonly Dictionary<int, IndexedSet<Collider>> _colliders = new();
    private readonly Dictionary<Entity, IndexedSet<ICollisionHandler>> _handlers = new();
    private Dictionary<(Collider, Collider), ContactInfo> _contacts = new();
    private Dictionary<(Collider, Collider), ContactInfo> _previousContacts = new();

    // Used for computing layer pairwise combinations.
    private readonly List<int> _layerBuf = new();

    /// <summary>
    /// AllLayers is special-cased and their presence/absence from this collection does not matter.
    /// </summary>
    public ReadOnlySpan<(int, int)> CollidableLayers =>
        CollectionsMarshal.AsSpan(_collidableLayers);

    public void MarkLayersCollidable(int layer1, int layer2)
    {
        Debug.Assert(layer1 != Collider.AllLayers && layer2 != Collider.AllLayers);
        var pair = (Math.Min(layer1, layer2), Math.Max(layer1, layer2));
        if (!_collidableLayers.Contains(pair))
        {
            _collidableLayers.Add(pair);
        }
    }

    public void MarkLayersNoncollidable(int layer1, int layer2)
    {
        Debug.Assert(layer1 != Collider.AllLayers && layer2 != Collider.AllLayers);
        var pair = (Math.Min(layer1, layer2), Math.Max(layer1, layer2));
        _collidableLayers.Remove(pair);
    }

    private bool LayersAreCollidable(int layer1, int layer2)
    {
        if (layer1 == Collider.AllLayers || layer2 == Collider.AllLayers)
        {
            return true;
        }
        return _collidableLayers.Contains((Math.Min(layer1, layer2), Math.Max(layer1, layer2)));
    }

    private void CheckContacts()
    {
        (_contacts, _previousContacts) = (_previousContacts, _contacts);
        _contacts.Clear();

        // Check same layer collisions.
        foreach (var layer in _colliders.Keys)
        {
            if (!LayersAreCollidable(layer, layer))
            {
                continue;
            }
            var colliders = _colliders[layer];
            for (var i = 0; i < colliders.Count - 1; i++)
            {
                for (var j = i + 1; j < colliders.Count; j++)
                {
                    CheckContactBetween(colliders[i], colliders[j]);
                }
            }
        }

        // Check cross-layer collisions.
        _layerBuf.Clear();
        foreach (var layer in _colliders.Keys)
        {
            _layerBuf.Add(layer);
        }
        for (var i = 0; i < _layerBuf.Count - 1; i++)
        {
            var l1 = _layerBuf[i];
            for (var j = i + 1; j < _layerBuf.Count; j++)
            {
                var l2 = _layerBuf[j];
                if (!LayersAreCollidable(l1, l2))
                {
                    continue;
                }
                foreach (var c1 in _colliders[l1])
                {
                    foreach (var c2 in _colliders[l2])
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
        if (a.GetOverlap(b) is not RectangleF overlap)
        {
            return;
        }
        if (a.Entity.Id > b.Entity.Id)
        {
            (a, b) = (b, a);
        }

        Vector2 normal;
        if (overlap.Width < overlap.Height)
        {
            if (a.AsWorldRect().Center.X < overlap.Center.X)
            {
                normal = new Vector2(-1, 0);
            }
            else
            {
                normal = new Vector2(1, 0);
            }
        }
        else
        {
            if (a.AsWorldRect().Center.Y < overlap.Center.Y)
            {
                normal = new Vector2(0, -1);
            }
            else
            {
                normal = new Vector2(0, 1);
            }
        }

        _contacts.Add((a, b), new ContactInfo(a, b, overlap, normal));
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
    /// Convenience method that returns an empty set if there are no handlers for the given entity.
    /// </summary>
    private ReadOnlyIndexedSet<ICollisionHandler> GetHandlers(Entity entity)
    {
        if (_handlers.TryGetValue(entity, out var handlers))
        {
            return new(handlers);
        }
        return ReadOnlyIndexedSet<ICollisionHandler>.Empty;
    }
}

public partial class CollisionManager : IUpdatable
{
    public const int UpdateOrder = 100;
    int IUpdatable.UpdateOrder => UpdateOrder;

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
        foreach (var component in entityChangelist.Detached)
        {
            if (component is Collider collider)
            {
                _colliders[collider.Layer].RemoveOrDie(collider);
            }
            if (component is ICollisionHandler handler)
            {
                _handlers[handler.Entity].RemoveOrDie(handler);
            }
        }
        foreach (var component in entityChangelist.Attached)
        {
            if (component is Collider collider)
            {
                _colliders.GetOrAddNew(collider.Layer).AddOrDie(collider);
            }
            if (component is ICollisionHandler handler)
            {
                _handlers.GetOrAddNew(handler.Entity).AddOrDie(handler);
            }
        }

        foreach (var entity in entityChangelist.Destroyed)
        {
            _handlers.Remove(entity);
        }
    }
}
