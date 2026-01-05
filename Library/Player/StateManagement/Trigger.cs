namespace Library.Player;

/// <summary>
/// Triggers with higher values have higher priority.
/// </summary>
public enum Trigger
{
    None,
    EnsureGrounded,
    EnsureUngrounded,
    Jump,
}
