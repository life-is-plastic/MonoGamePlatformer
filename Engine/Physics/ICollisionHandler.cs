using Engine.Core;

namespace Engine.Physics;

/// <summary>
/// Executes in response to collisions involving a collider sibling of this handler component.
/// </summary>
public interface ICollisionHandler : IComponent
{
    /// <summary>
    /// Invoked on the first frame the two colliders began overlapping.
    /// </summary>
    public void OnCollisionEnter(in ContactInfo contact) { }

    /// <summary>
    /// Invoked starting on the second frame since the two colliders began overlapping.
    /// </summary>
    public void OnCollisionStay(in ContactInfo contact) { }

    /// <summary>
    /// Invoked on the first frame the two colliders stopped overlapping.
    /// </summary>
    public void OnCollisionExit(in ContactInfo finalContact) { }
}
