using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Engine.Util;

/// <summary>
/// Like <c>Microsoft.Xna.Framework.Rectangle</c> but uses floats underneath.
/// </summary>
public readonly record struct RectangleF
{
    public Vector2 Location { get; }
    public Vector2 Size { get; }
    public Vector2 Center => Location + Size / 2;
    public float X => Location.X;
    public float Y => Location.Y;
    public float Width => Size.X;
    public float Height => Size.Y;
    public float Left => X;
    public float Right => X + Width;
    public float Top => Y;
    public float Bottom => Y + Height;

    public RectangleF(Vector2 location, Vector2 size)
    {
        Debug.Assert(size.X >= 0 && size.Y >= 0);
        Location = location;
        Size = size;
    }

    public RectangleF(float x, float y, float width, float height)
        : this(new Vector2(x, y), new Vector2(width, height)) { }

    public RectangleF WithLocation(Vector2 location) => new(location, Size);

    public RectangleF WithLocation(float? x = null, float? y = null) =>
        WithLocation(new Vector2(x ?? X, y ?? Y));

    public RectangleF WithSize(Vector2 size) => new(Location, size);

    public RectangleF WithSize(float? width = null, float? height = null) =>
        WithSize(new Vector2(width ?? Width, height ?? Height));

    public RectangleF WithCenter(Vector2 center) => new(center - Size / 2, Size);

    public RectangleF WithCenter(float? x = null, float? y = null) =>
        WithCenter(new Vector2(x ?? X, y ?? Y));

    public RectangleF Translate(Vector2 displacement) => new(Location + displacement, Size);

    public RectangleF Scale(Vector2 scale) => new(Location, Size * scale);

    public RectangleF ScaleFromCenter(Vector2 scale) => Scale(scale).WithCenter(Center);

    public bool Overlaps(RectangleF other)
    {
        return Left < other.Right && other.Left < Right && Top < other.Bottom && other.Top < Bottom;
    }

    /// <summary>
    /// Returns the overlap if it exists.
    /// </summary>
    public RectangleF? GetOverlap(RectangleF other)
    {
        if (!Overlaps(other))
        {
            return null;
        }

        var left = Math.Max(Left, other.Left);
        var top = Math.Max(Top, other.Top);
        var right = Math.Min(Right, other.Right);
        var bottom = Math.Min(Bottom, other.Bottom);
        return new(left, top, right - left, bottom - top);
    }

    public Rectangle ToRectangle()
    {
        return new Rectangle(
            (int)Math.Round(Left),
            (int)Math.Round(Top),
            (int)Math.Round(Width),
            (int)Math.Round(Height)
        );
    }
}
