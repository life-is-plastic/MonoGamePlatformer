using System;
using Microsoft.Xna.Framework;

namespace Engine.Util;

public static class Vector2Extenions
{
    extension(Vector2 vector)
    {
        public Vector2 WithX(float x) => new(x, vector.Y);

        public Vector2 WithY(float y) => new(vector.X, y);

        public float Rotation() => MathF.Atan2(vector.Y, vector.X);

        public float PerpDot(Vector2 other) => vector.X * other.Y - vector.Y * other.X;

        public Vector2 SmoothStep(Vector2 other, float t) =>
            new(
                MathHelper.SmoothStep(vector.X, other.X, t),
                MathHelper.SmoothStep(vector.Y, other.Y, t)
            );
    }
}
