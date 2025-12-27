using System;
using System.Diagnostics;

namespace Engine.Core;

/// <summary>
/// Base class for all components.
/// </summary>
public abstract partial class Component : IComponent
{
    public const int DefaultIndex = int.MinValue;

    public Entity Entity { get; private set; } = null!;
    public int ComponentIndex { get; init; } = DefaultIndex;

    void IComponent.SetEntity(Entity entity)
    {
        // Only allow setting once.
        Debug.Assert(Entity is null);
        Entity = entity;
    }

    public virtual void Begin() { }

    public virtual void End() { }
}

// Convenience utilities for implementing components.
public abstract partial class Component
{
    protected Scene Scene => Entity.Scene;

    protected T GetSingleton<T>()
        where T : IComponent
    {
        return Scene.Singletons.Get<T>();
    }

    protected Entity StageCreate(string name)
    {
        return Scene.EntityChangelist.StageCreate(name);
    }

    protected EntityHandle GetEntityWith<T>()
        where T : IComponent
    {
        return GetEntityWith(typeof(T));
    }

    protected EntityHandle GetEntityWith<T1, T2>()
    {
        return GetEntityWith(typeof(T1), typeof(T2));
    }

    protected EntityHandle GetEntityWith<T1, T2, T3>()
    {
        return GetEntityWith(typeof(T1), typeof(T2), typeof(T3));
    }

    protected EntityHandle GetEntityWith(params ReadOnlySpan<Type> componentTypes)
    {
        return MaybeGetEntityWith(componentTypes)
            ?? throw new ArgumentException(
                $"no entities found with components [{string.Join(", ", componentTypes)}]"
            );
    }

    /// <summary>
    /// Finds and returns an entity containing all the given component types (and at the default
    /// component index). If there are multiple candidate entities, there is no guarantee which one
    /// is chosen.
    /// </summary>
    protected EntityHandle? MaybeGetEntityWith(params ReadOnlySpan<Type> componentTypes)
    {
        foreach (var entity in Scene.Entities)
        {
            foreach (var type in componentTypes)
            {
                if (!entity.Has(type, DefaultIndex))
                {
                    goto NextEntity;
                }
            }
            return new(entity);
            NextEntity:
            ;
        }
        return null;
    }
}
