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

    protected virtual void Begin() { }

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
