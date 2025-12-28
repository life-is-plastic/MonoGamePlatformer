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

    /// <summary>
    /// Invoked when this component is formally attached to its owning entity. Useful for
    /// post-construction initialization of data that depends on having access to the containing
    /// scene.
    /// </summary>
    internal void Begin();

    /// <summary>
    /// Invoked on scene disposal or when detaching this component from its owning entity. Useful
    /// for cleaning up data that should not wait for the garbage collector.
    /// </summary>
    internal void End();

    /// <summary>
    /// Only called by <c>EntityChangelist.StageAttach()</c>.
    /// </summary>
    internal void SetEntity(Entity entity);
}
