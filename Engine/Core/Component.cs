using System.Diagnostics;

namespace Engine.Core;

/// <summary>
/// Base class for all components.
/// <para>Avoid caching direct references to components. Wrap them in a <c>ComponentHandle</c>
/// instead.</para>
/// </summary>
public abstract class Component : IComponent
{
    public const int DefaultIndex = int.MinValue;

    public Scene Scene => Entity.Scene;
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
