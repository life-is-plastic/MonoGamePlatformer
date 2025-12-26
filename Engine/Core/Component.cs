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

    /// <summary>
    /// Finds an entity containing all the given component types (and at the default component
    /// index), and returns a handle to it. If there are multiple candidate entities, there is no
    /// guarantee which one is chosen.
    /// </summary>
    protected EntityHandle? FindEntity(params ReadOnlySpan<Type> componentTypes)
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
