using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Engine.Core;

/// <summary>
/// A dictionary of components, keyed by (concrete type, index) pairs. Index facilitates attaching
/// multiple instances of the same component type. Avoid attaching/detaching/lookups with index if a
/// component should only appear at most once per entity.
/// </summary>
public class Entity
{
    private const int DefaultIndex = int.MinValue;

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
        return Has<T>(DefaultIndex);
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
        return Get<T>(DefaultIndex);
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
            $"{this} does not have a component of type {type}{(index == DefaultIndex ? "" : $" at index {index}")}"
        );
    }

    public T? MaybeGet<T>()
        where T : IComponent
    {
        return MaybeGet<T>(DefaultIndex);
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

    public Entity StageAttach<T>(T component)
        where T : IComponent
    {
        return StageAttach(component, DefaultIndex);
    }

    public Entity StageAttach<T>(T component, int index)
        where T : IComponent
    {
        Scene.EntityChangelist.StageAttach(this, component, index);
        return this;
    }

    public Entity StageDetach<T>()
        where T : IComponent
    {
        return StageDetach<T>(DefaultIndex);
    }

    public Entity StageDetach<T>(int index)
        where T : IComponent
    {
        Scene.EntityChangelist.StageDetach(this, typeof(T), index);
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
    public void ImmediatelyAttach(IComponent component, int index)
    {
        Debug.Assert(component.Entity == this);
        var added = _components.TryAdd((component.GetType(), index), component);
        Debug.Assert(
            added,
            $"{this} already has a component of type {component.GetType()}{(index == DefaultIndex ? "" : $" at index {index}")}"
        );
    }

    /// <summary>
    /// Immediately attaches a component to this entity without cleaning up the component from the
    /// scene.
    /// </summary>
    public void ImmediatelyDetach(Type type, int index)
    {
        var removed = _components.Remove((type, index));
        Debug.Assert(
            removed,
            $"{this} does not have a component of type {type}{(index == DefaultIndex ? "" : $" at index {index}")}"
        );
    }
}
