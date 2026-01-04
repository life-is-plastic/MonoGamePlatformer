using System;
using Microsoft.Xna.Framework;

namespace Library.PlayerManagement;

public readonly record struct LateralMotionHelper
{
    public static float DefaultMaxSpeed => 150;

    public float MaxSpeed { get; init; } = DefaultMaxSpeed;
    public float TimeToMaxSpeed { get; init; } = 0.25f;
    public float TimeToFullStop { get; init; } = 0.125f;
    public float Acceleration => MaxSpeed / TimeToMaxSpeed;
    public float Friction => MaxSpeed / TimeToFullStop;

    public LateralMotionHelper() { }

    public float NextVelocity(float currentVelocity, float normalizedInput, float deltaTime)
    {
        float dv;

        if (Math.Abs(normalizedInput) > 0.005)
        {
            dv = currentVelocity * normalizedInput >= 0 ? Acceleration : Friction;
            dv *= deltaTime;
            return MathHelper.Clamp(
                currentVelocity + MathF.CopySign(dv, normalizedInput),
                -MaxSpeed,
                MaxSpeed
            );
        }

        dv = Friction * deltaTime;
        if (dv < Math.Abs(currentVelocity))
        {
            return MathHelper.Clamp(
                currentVelocity - MathF.CopySign(dv, currentVelocity),
                -MaxSpeed,
                MaxSpeed
            );
        }

        return 0;
    }
}
