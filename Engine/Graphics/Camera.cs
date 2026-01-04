using System;
using System.Diagnostics;
using Engine.Util;
using Microsoft.Xna.Framework;

namespace Engine;

/// <summary>
/// Zoom is determined by the sibling transform component's scale. Scale > 1 means zoom in (i.e.
/// smaller camera viewport) and scale < 1 means zoom out.
/// </summary>
public class Camera : Component
{
    /// <summary>
    /// Base camera viewport dimensions before zoom calculations.
    /// </summary>
    public Point Size
    {
        get;
        init
        {
            Debug.Assert(value.X > 0);
            Debug.Assert(value.Y > 0);
            field = value;
        }
    } = new(320, 200);

    /// <summary>
    /// Base camera viewport width before zoom calculations.
    /// </summary>
    public int Width => Size.X;

    /// <summary>
    /// Base camera viewport height before zoom calculations.
    /// </summary>
    public int Height => Size.Y;

    /// <summary>
    /// A number that, when multiplied with camera viewport dimensions, stretches the viewport to
    /// fit the screen while preserving aspect ratio. The result assumes no zooming.
    /// </summary>
    public float ScreenScale =>
        Math.Min(
            (float)Scene.Game.GraphicsDevice.Viewport.Width / Width,
            (float)Scene.Game.GraphicsDevice.Viewport.Height / Height
        );

    /// <summary>
    /// Returns the world space rectangle representing this camera's viewport.
    /// </summary>
    public RectangleF AsWorldRectangleF()
    {
        var transform = Entity.Get<Transform>();
        return new RectangleF() { Size = Size.ToVector2() / transform.Scale }.WithCenter(
            transform.Position
        );
    }
}
