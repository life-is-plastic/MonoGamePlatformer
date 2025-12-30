using System;
using System.Diagnostics;
using Engine.Core;
using Engine.Util;
using Microsoft.Xna.Framework;

namespace Engine.Graphics;

/// <summary>
/// Zoom is determined by the sibling transform component's scale. Scale > 1 means zoom in (i.e.
/// smaller camera viewport) and scale < 1 means zoom out.
/// </summary>
public class Camera : Component
{
    /// <summary>
    /// Base camera viewport width before zoom calculations.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Base camera viewport height before zoom calculations.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Base camera viewport dimensions before zoom calculations.
    /// </summary>
    public Point Size => new(Width, Height);

    /// <summary>
    /// A number that, when multiplied with camera viewport dimensions, stretches the viewport to
    /// fit the screen while preserving aspect ratio. The result assumes no zooming.
    /// </summary>
    public float ScreenScale =>
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

    /// <summary>
    /// Returns the world space rectangle representing this camera's viewport.
    /// </summary>
    public RectangleF AsWorldRectangleF()
    {
        var transform = Entity.Get<Transform>();
        return new RectangleF(
            0,
            0,
            Width / transform.Scale.X,
            Height / transform.Scale.Y
        ).WithCenter(transform.Position);
    }
}
