using System;
using Microsoft.Xna.Framework;

namespace Library.Player;

public readonly record struct LateralHelper
{
    public float MaxSpeed { get; init; } = 80;

    /// <summary>
    /// From being stationary.
    /// </summary>
    public float TimeToMaxSpeed { get; init; } = 0.125f;

    /// <summary>
    /// From max speed.
    /// </summary>
    public float TimeToFullStop { get; init; } = 0.0625f;

    public float SameDirectionAcceleration => MaxSpeed / TimeToMaxSpeed;
    public float OppositeDirectionAcceleration => MaxSpeed / TimeToFullStop;

    public LateralHelper() { }

    public float NextVelocity(float currentVelocity, int input, float deltaTime)
    {
        if (input != 0)
        {
            var accel =
                currentVelocity * input >= 0
                    ? SameDirectionAcceleration
                    : OppositeDirectionAcceleration;
            return MathHelper.Clamp(
                currentVelocity + MathF.CopySign(accel * deltaTime, input),
                -MaxSpeed,
                MaxSpeed
            );
        }

        var dv = OppositeDirectionAcceleration * deltaTime;
        if (dv >= Math.Abs(currentVelocity))
        {
            return 0;
        }
        return MathHelper.Clamp(
            currentVelocity - MathF.CopySign(dv, currentVelocity),
            -MaxSpeed,
            MaxSpeed
        );
    }
}
