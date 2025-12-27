namespace Engine.Core;

/// <summary>
/// Interface for components that update every frame.
/// </summary>
public interface IUpdatable : IComponent
{
    /// <summary>
    /// Higher value means updated later. All instances of the same concrete type should have the
    /// same update order; in other words, implement this like a class const and do not mutate it.
    /// </summary>
    public int UpdateOrder => 0;

    /// <summary>
    /// Invoked on scene pause.
    /// </summary>
    /// <returns>True if this component's <c>Update()</c> should be blocked while paused.</returns>
    public bool Pause() => true;

    /// <summary>
    /// Invoked on scene resume. This gets called regardless of <c>Pause()</c>'s return value.
    /// </summary>
    public void Unpause() { }

    /// <summary>
    /// Invoked every frame, or every unpaused frame if <c>Pause()</c> returns true.
    /// </summary>
    public void Update() { }
}
