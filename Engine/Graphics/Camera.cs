using System.Diagnostics;
using Engine.EC;
using Microsoft.Xna.Framework;

namespace Engine.Graphics;

public class Camera : Component
{
    public int Width { get; }
    public int Height { get; }
    public Point Size => new(Width, Height);

    public Camera()
        : this(320, 200) { }

    public Camera(int width, int height)
    {
        Debug.Assert(width > 80);
        Debug.Assert(height > 60);
        Width = width;
        Height = height;
    }
}
