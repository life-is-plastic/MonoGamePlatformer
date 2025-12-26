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
}
