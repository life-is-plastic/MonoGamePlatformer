using System;
using System.Collections.Generic;
using System.Diagnostics;
using Engine.Util.Collections;

namespace Engine.Core;

/// <summary>
/// A staging area for entity/component additions/removals, which are applied at the beginning of
/// the next frame.
/// </summary>
public sealed class EntityChangelist
{
    private int _nextEntityId = 1;
    private readonly IndexedSet<IEntitySyncer> _syncers = new();

    private readonly IndexedSet<Entity> _created = new();
    private readonly IndexedSet<Entity> _destroyed = new();
    private readonly Dictionary<(Entity, Type, int), IComponent> _attached = new();
    private readonly Dictionary<(Entity, Type, int), IComponent> _detached = new();

    public IndexedSetView<Entity> Created => new(_created);
    public IndexedSetView<Entity> Destroyed => new(_destroyed);
    public DictionaryView<(Entity, Type, int), IComponent> Attached => new(_attached);
    public DictionaryView<(Entity, Type, int), IComponent> Detached => new(_detached);

    internal Entity StageCreate(Scene scene, string name)
    {
        var entity = new Entity(scene, _nextEntityId++, name);
        _created.AddOrDie(entity);
        return entity;
    }

    internal void StageDestroy(Entity entity)
    {
        _destroyed.Add(entity);
        foreach (var (_, component) in entity)
        {
            StageDetach(entity, component);
        }
    }

    internal void StageAttach(Entity entity, IComponent component)
    {
        Debug.Assert(!entity.Has(component.GetType(), component.ComponentIndex));
        _attached[(entity, component.GetType(), component.ComponentIndex)] = component;
        component.SetEntity(entity);
    }

    internal void StageDetach(Entity entity, IComponent component)
    {
        Debug.Assert(entity.Get(component.GetType(), component.ComponentIndex) == component);
        _detached[(entity, component.GetType(), component.ComponentIndex)] = component;
    }

    /// <summary>
    /// Writes staged changes to the given entity set and entity updater, then clears the internal
    /// staging areas.
    /// </summary>
    internal void Apply(IndexedSet<Entity> entities, EntityUpdater entityUpdater)
    {
        ProcessRemovals(entities, entityUpdater);
        ProcessAdditions(entities, entityUpdater);
        Sync();
        Clear();
    }

    private void ProcessAdditions(IndexedSet<Entity> entities, EntityUpdater entityUpdater)
    {
        foreach (var entity in _created)
        {
            entities.AddOrDie(entity);
        }
        foreach (var ((entity, _, _), component) in _attached)
        {
            entity.ImmediatelyAttach(component);
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

    private void ProcessRemovals(IndexedSet<Entity> entities, EntityUpdater entityUpdater)
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
        foreach (var ((entity, _, _), component) in _detached)
        {
            entity.ImmediatelyDetach(component);
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
