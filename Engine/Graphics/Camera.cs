using Engine.Core;
using Microsoft.Xna.Framework;

namespace Engine.Graphics;

public class Camera : Component
{
    public int Width { get; } = 360;
    public int Height { get; } = 200;
    public Point Size => new(Width, Height);
}
