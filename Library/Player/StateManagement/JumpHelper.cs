namespace Library.Player;

public readonly record struct JumpHelper
{
    public static JumpHelper Default { get; } = new() { Height = 26, TimeToPeak = 0.3f };
    public static JumpHelper HeldInput { get; } =
        new() { Height = 50, TimeToPeak = -2 * 50 / Default.InitialVelocity };

    /// <summary>
    /// Base jump height.
    /// </summary>
    public required float Height { get; init; }

    /// <summary>
    /// Time between jumping and reaching peak height.
    /// </summary>
    public required float TimeToPeak { get; init; }

    public float Gravity => 2 * Height / (TimeToPeak * TimeToPeak);
    public float InitialVelocity => -2 * Height / TimeToPeak;
}
