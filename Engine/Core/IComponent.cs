namespace Engine.Core;

/// <summary>
/// Base interface for all components. This exists separately from the <c>Component</c> class to be
/// inherited by other component interfaces.
/// <para>This interface is unimplementable by external assemblies, due to the presence of internal
/// methods.</para>
/// </summary>
public interface IComponent
{
    /// <summary>
    /// The entity that owns this component.
    /// </summary>
    public Entity Entity { get; }

    /// <summary>
    /// Forms, together with component type, a composite key that uniquely identifies this component
    /// within its owning entity.
    /// </summary>
    public int ComponentIndex { get; }

    internal void Begin();

    internal void End();

    internal void SetEntity(Entity entity);
}
