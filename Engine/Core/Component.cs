using System.Diagnostics;

namespace Engine.Core;

/// <summary>
/// Base class for all components.
/// <para>Do not cache component references. Always access components via their owning
/// entity.</para>
/// </summary>
public abstract class Component : IComponent
{
    public const int DefaultIndex = int.MinValue;

    public Entity Entity { get; private set; } = null!;
    public int ComponentIndex { get; init; } = DefaultIndex;
    protected Scene Scene => Entity.Scene;

    public override string ToString()
    {
        return $"[{Entity} -> ({GetType().Name}, {ComponentIndex})]";
    }

    /// <summary>
    /// Invoked when this component is formally attached to its owning entity. Useful for
    /// post-construction initialization of data that depends on having access to the containing
    /// scene.
    /// </summary>
    protected virtual void Begin() { }

    /// <summary>
    /// Invoked on scene disposal or when detaching this component from its owning entity. Useful
    /// for cleaning up data that should not wait for the garbage collector.
    /// </summary>
    protected virtual void End() { }

    void IComponent.Begin()
    {
        Begin();
    }

    void IComponent.End()
    {
        End();
    }

    void IComponent.SetEntity(Entity entity)
    {
        Debug.Assert(Entity is null);
        Entity = entity;
    }
}
