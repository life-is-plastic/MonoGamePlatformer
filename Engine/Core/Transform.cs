using Microsoft.Xna.Framework;

namespace Engine.Core;

public class Transform : Component
{
    public Vector2 Position = default;

    /// <summary>
    /// Currently only for drawing. Completedly ignored by physics and camera.
    /// </summary>
    public float Rotation
    {
        get;
        set => field = MathHelper.WrapAngle(value);
    }

    public Vector2 Scale = new(1, 1);
}
