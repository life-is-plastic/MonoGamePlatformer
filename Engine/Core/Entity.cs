using System;
using System.Collections.Generic;
using System.Diagnostics;
using Engine.Util.Extensions;

namespace Engine.Core;

/// <summary>
/// A dictionary of components, keyed by (concrete type, index) pairs. Index facilitates attaching
/// multiple instances of the same component type. Avoid attaching/detaching/lookups with index if a
/// component should only appear at most once per entity.
/// </summary>
public class Entity
{
    private readonly Dictionary<(Type, int), IComponent> _components = new();

    public Dictionary<(Type, int), IComponent>.ValueCollection Components => _components.Values;

    /// <summary>
    /// The scene owning this entity.
    /// </summary>
    public Scene Scene { get; }

    /// <summary>
    /// Uniquely identifies this entity within its containing scene.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Human readable name for debugging.
    /// </summary>
    public string Name { get; }

    public Entity(Scene scene, int id, string name)
    {
        Scene = scene;
        Id = id;
        Name = name;
    }

    public override string ToString()
    {
        return $"{typeof(Entity).Name}({Id}, {Name})";
    }

    public Dictionary<(Type, int), IComponent>.Enumerator GetEnumerator()
    {
        return _components.GetEnumerator();
    }

    public bool Has<T>()
        where T : IComponent
    {
        return Has<T>(Component.DefaultIndex);
    }

    public bool Has<T>(int index)
        where T : IComponent
    {
        return Has(typeof(T), index);
    }

    public bool Has(Type type, int index)
    {
        Debug.Assert(type.IsAssignableTo(typeof(IComponent)));
        return _components.ContainsKey((type, index));
    }

    public T Get<T>()
        where T : IComponent
    {
        return Get<T>(Component.DefaultIndex);
    }

    public T Get<T>(int index)
        where T : IComponent
    {
        return (T)Get(typeof(T), index);
    }

    public IComponent Get(Type type, int index)
    {
        if (MaybeGet(type, index) is IComponent component)
        {
            return component;
        }
        throw new ArgumentException(
            $"{this} does not have a component of type {type}{(index == Component.DefaultIndex ? "" : $" at index {index}")}"
        );
    }

    public T? MaybeGet<T>()
        where T : IComponent
    {
        return MaybeGet<T>(Component.DefaultIndex);
    }

    public T? MaybeGet<T>(int index)
        where T : IComponent
    {
        return (T?)MaybeGet(typeof(T), index);
    }

    public IComponent? MaybeGet(Type type, int index)
    {
        Debug.Assert(type.IsAssignableTo(typeof(IComponent)));
        return _components.TryGetValue((type, index), out var component) ? component : null;
    }

    public Entity StageAttach(IComponent component)
    {
        Scene.EntityChangelist.StageAttach(this, component);
        return this;
    }

    public Entity StageDetach(IComponent component)
    {
        Scene.EntityChangelist.StageDetach(this, component);
        return this;
    }

    public Entity StageDestroy()
    {
        Scene.EntityChangelist.StageDestroy(this);
        return this;
    }

    /// <summary>
    /// Whether or not this entity is part of its containing scene.
    /// </summary>
    /// <returns>
    /// True starting from the frame after <c>EntityChangelist.StageCreate()</c>. False before that
    /// frame or starting on the frame after <c>EntityChangelist.StageDestroy()</c>.
    /// </returns>
    public bool IsAlive()
    {
        return Scene.Entities.Contains(this);
    }

    /// <summary>
    /// Immediately attaches a component to this entity without integrating the component with the
    /// scene.
    /// </summary>
    public void ImmediatelyAttach(IComponent component)
    {
        Debug.Assert(component.Entity == this);
        var added = _components.TryAdd((component.GetType(), component.ComponentIndex), component);
        Debug.Assert(
            added,
            $"{this} already has a component of type {component.GetType()}{(component.ComponentIndex == Component.DefaultIndex ? "" : $" at index {component.ComponentIndex}")}"
        );
    }

    /// <summary>
    /// Immediately attaches a component to this entity without cleaning up the component from the
    /// scene.
    /// </summary>
    public void ImmediatelyDetach(IComponent component)
    {
        Debug.Assert(Get(component.GetType(), component.ComponentIndex) == component);
        _components.RemoveOrDie((component.GetType(), component.ComponentIndex));
    }
}
