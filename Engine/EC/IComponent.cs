namespace Engine.EC;

/// <summary>
/// Base interface for all components. This exists separately from the <c>Component</c> class to be
/// inherited by other component interfaces.
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
    /// This gets called on staging the component for attachment, and should not be called again
    /// from thereon.
    /// </summary>
    public void SetEntity(Entity entity);

    /// <summary>
    /// Invoked when this component is formally attached to its owning entity. Useful for
    /// post-construction initialization of data that depends on having access to the containing
    /// scene.
    /// </summary>
    public void Begin();

    /// <summary>
    /// Invoked on scene disposal or when detaching this component from its owning entity. Useful
    /// for cleaning up data that should not wait for the garbage collector.
    /// </summary>
    public void End();
}
