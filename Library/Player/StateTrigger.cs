namespace Library.Player;

/// <summary>
/// Triggers with higher values have higher priority.
/// </summary>
public enum StateTrigger
{
    None,
    EnsureGrounded,
    EnsureUngrounded,
    Jump,
}
