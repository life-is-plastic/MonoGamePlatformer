using Microsoft.Xna.Framework;

namespace Engine.Core;

public class Transform : Component
{
    public Vector2 Position = default;
    public Vector2 Scale = new(1, 1);
}
