using System.Diagnostics;

namespace Engine.Core;

/// <summary>
/// Base class for all components.
/// </summary>
public abstract partial class Component : IComponent
{
    public Entity Entity { get; private set; } = null!;

    // Only allow setting once.
    protected virtual void SetEntity(Entity entity)
    {
        Debug.Assert(Entity is null);
        Entity = entity;
    }

    void IComponent.SetEntity(Entity entity)
    {
        SetEntity(entity);
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
}
