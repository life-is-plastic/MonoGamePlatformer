using Microsoft.Xna.Framework;

namespace Engine.Util.Extensions;

public static class Vector2Extenions
{
    extension(Vector2 vector)
    {
        public Vector2 WithX(float x) => new(x, vector.Y);

        public Vector2 WithY(float y) => new(vector.X, y);

        public float PerpDot(Vector2 other) => vector.X * other.Y - vector.Y * other.X;
    }
}
