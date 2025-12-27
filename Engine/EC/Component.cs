using System;
using System.Diagnostics;
using Engine.Application;

namespace Engine.EC;

/// <summary>
/// Base class for all components.
/// <para>Avoid caching direct references to components. Wrap them in a <c>ComponentHandle</c>
/// instead.</para>
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

    protected (T Component, Entity Entity) FindByComponent<T>()
        where T : IComponent
    {
        Span<IComponent> buf = [null!];
        var entity = FindByComponent(buf, typeof(T));
        return ((T)buf[0], entity);
    }

    protected (T1 Component1, T2 Component2, Entity Entity) FindByComponent<T1, T2>()
        where T1 : IComponent
        where T2 : IComponent
    {
        Span<IComponent> buf = [null!, null!];
        var entity = FindByComponent(buf, typeof(T1), typeof(T2));
        return ((T1)buf[0], (T2)buf[1], entity);
    }

    private Entity FindByComponent(Span<IComponent> buf, params ReadOnlySpan<Type> componentTypes)
    {
        return MaybeFindByComponent(buf, componentTypes)
            ?? throw new ArgumentException(
                $"no entities found with components [{string.Join(", ", componentTypes)}]"
            );
    }

    /// <summary>
    /// Finds and returns an entity containing all the given component types (and at the default
    /// component index). If there are multiple candidate entities, there is no guarantee which one
    /// is chosen.
    /// </summary>
    private Entity? MaybeFindByComponent(
        Span<IComponent> buf,
        params ReadOnlySpan<Type> componentTypes
    )
    {
        Debug.Assert(buf.Length == componentTypes.Length);
        foreach (var entity in Scene.Entities)
        {
            for (var i = 0; i < componentTypes.Length; i++)
            {
                if (entity.MaybeGet(componentTypes[i], DefaultIndex) is { } component)
                {
                    buf[i] = component;
                }
                else
                {
                    goto NextEntity;
                }
            }
            return entity;
            NextEntity:
            ;
        }
        return null;
    }
}
