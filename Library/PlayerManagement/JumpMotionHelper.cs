namespace Library.PlayerManagement;

public readonly record struct JumpMotionHelper
{
    public float MaxLateralSpeed { get; init; } = LateralMotionHelper.DefaultMaxSpeed;

    /// <summary>
    /// Horizontal distance covered when jumping at max lateral speed across.
    /// </summary>
    public float MaxWidth { get; init; } = 64;

    /// <summary>
    /// Base jump height.
    /// </summary>
    public float Height { get; init; } = 32;

    public float TimeToPeak => MaxWidth / MaxLateralSpeed / 2;

    // From https://www.youtube.com/watch?v=hG9SzQxaCm8, these two should be multiplied by 2, but
    // for some reason leaving the that multiplication makes the simulation more accurate.
    public float Gravity => Height / (TimeToPeak * TimeToPeak);
    public float InitialVelocity => -Height / TimeToPeak;

    public JumpMotionHelper() { }
}
