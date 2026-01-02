using Microsoft.Xna.Framework;

namespace Engine;

public class Transform : Component
{
    public Vector2 Position = new();

    /// <summary>
    /// Currently only used for drawing. Completedly ignored by physics and camera.
    /// </summary>
    public float Rotation
    {
        get;
        set => field = MathHelper.WrapAngle(value);
    }

    public Vector2 Scale = new(1);
}
