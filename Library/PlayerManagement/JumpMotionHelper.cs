namespace Library.PlayerManagement;

public readonly record struct JumpMotionHelper
{
    /// <summary>
    /// Base jump height.
    /// </summary>
    public float Height { get; init; } = 70;

    /// <summary>
    /// Time between jumping and reaching peak height.
    /// </summary>
    public float TimeToPeak { get; init; } = 0.4f;

    // From https://www.youtube.com/watch?v=hG9SzQxaCm8, these two should be multiplied by 2, but
    // for some reason leaving the that multiplication makes the simulation more accurate.
    public float Gravity => Height / (TimeToPeak * TimeToPeak);
    public float InitialVelocity => -Height / TimeToPeak;

    public JumpMotionHelper() { }
}
