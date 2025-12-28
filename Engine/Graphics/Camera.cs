using System;
using System.Diagnostics;
using Engine.Core;
using Engine.Util;
using Microsoft.Xna.Framework;

namespace Engine.Graphics;

public class Camera : Component
{
    public int Width { get; }
    public int Height { get; }
    public Point Size => new(Width, Height);
    public float ViewportScale =>
        Math.Min(
            (float)Scene.Game.GraphicsDevice.Viewport.Width / Width,
            (float)Scene.Game.GraphicsDevice.Viewport.Height / Height
        );

    public Camera()
        : this(320, 200) { }

    public Camera(int width, int height)
    {
        Debug.Assert(width > 0);
        Debug.Assert(height > 0);
        Width = width;
        Height = height;
    }

    public RectangleF AsWorldRectangleF()
    {
        var transform = Entity.Get<Transform>();
        return new RectangleF(default, Size.ToVector2()).WithCenter(transform.Position);
    }
}
