using System;
using System.Collections.Generic;
using System.Diagnostics;
using Engine.Util.Collections;

namespace Engine.Core;

/// <summary>
/// A staging area for entity/component additions/removals, which are applied at the beginning of
/// the next frame.
/// </summary>
public class EntityChangelist
{
    private int _nextEntityId = 1;
    private readonly Scene _scene;
    private readonly IndexedSet<IEntitySyncer> _syncers = new();

    private readonly IndexedSet<Entity> _created = new();
    private readonly IndexedSet<Entity> _destroyed = new();
    private readonly Dictionary<(Entity, Type, int), IComponent> _attached = new();
    private readonly Dictionary<(Entity, Type, int), IComponent> _detached = new();

    public ReadOnlyIndexedSet<Entity> Created => new(_created);
    public ReadOnlyIndexedSet<Entity> Destroyed => new(_destroyed);
    public Dictionary<(Entity, Type, int), IComponent>.ValueCollection Attached => _attached.Values;
    public Dictionary<(Entity, Type, int), IComponent>.ValueCollection Detached => _detached.Values;

    public EntityChangelist(Scene scene)
    {
        _scene = scene;
    }

    /// <summary>
    /// Creates an entity and stages it to become part of the scene at the beginning of the next
    /// frame.
    /// </summary>
    public Entity StageCreate(string name)
    {
        var entity = new Entity(_scene, _nextEntityId++, name);
        _created.AddOrDie(entity);
        return entity;
    }

    /// <summary>
    /// Stages an entity to be destroyed at the beginning of the next frame. Within a frame, this
    /// method is idempotent.
    /// </summary>
    public void StageDestroy(Entity entity)
    {
        _destroyed.Add(entity);
        foreach (var ((_, index), component) in entity)
        {
            StageDetach(entity, component.GetType(), index);
        }
    }

    /// <summary>
    /// Stages the given component to be attached at the beginning of the next frame. If multiple
    /// components of the same (type, index) are staged during the same frame, the final component
    /// will be the one actually attached.
    /// </summary>
    public void StageAttach(Entity entity, IComponent component, int index)
    {
        Debug.Assert(!entity.Has(component.GetType(), index));
        _attached[(entity, component.GetType(), index)] = component;
        component.SetEntity(entity);
    }

    /// <summary>
    /// Stages the given component to be detached at the beginning of the next frame. Within a
    /// frame, this method is idempotent.
    /// </summary>
    public void StageDetach(Entity entity, Type type, int index)
    {
        _detached[(entity, type, index)] = entity.Get(type, index);
    }

    /// <summary>
    /// Writes staged changes to the given entity set and entity updater, then clears the internal
    /// staging areas.
    /// </summary>
    public void Apply(in IndexedSet<Entity> entities, EntityUpdater entityUpdater)
    {
        ProcessRemovals(entities, entityUpdater);
        ProcessAdditions(entities, entityUpdater);
        Sync();
        Clear();
    }

    private void ProcessAdditions(in IndexedSet<Entity> entities, EntityUpdater entityUpdater)
    {
        foreach (var entity in _created)
        {
            entities.AddOrDie(entity);
        }
        foreach (var ((entity, _, index), component) in _attached)
        {
            entity.ImmediatelyAttach(component, index);
        }
        foreach (var component in _attached.Values)
        {
            component.Begin();
            if (component is IUpdatable updatable)
            {
                entityUpdater.Add(updatable);
            }
            if (component is IEntitySyncer syncer)
            {
                _syncers.AddOrDie(syncer);
            }
        }
    }

    private void ProcessRemovals(in IndexedSet<Entity> entities, EntityUpdater entityUpdater)
    {
        foreach (var component in _detached.Values)
        {
            component.End();
            if (component is IUpdatable updatable)
            {
                entityUpdater.Remove(updatable);
            }
            if (component is IEntitySyncer syncer)
            {
                _syncers.RemoveOrDie(syncer);
            }
        }
        foreach (var (entity, type, index) in _detached.Keys)
        {
            entity.ImmediatelyDetach(type, index);
        }
        foreach (var entity in _destroyed)
        {
            entities.RemoveOrDie(entity);
        }
    }

    private void Sync()
    {
        foreach (var syncer in _syncers)
        {
            syncer.Sync(this);
        }
    }

    private void Clear()
    {
        _created.Clear();
        _destroyed.Clear();
        _attached.Clear();
        _detached.Clear();
    }
}
