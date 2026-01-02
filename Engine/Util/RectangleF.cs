using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Engine.Util;

/// <summary>
/// Like <c>Microsoft.Xna.Framework.Rectangle</c> but uses floats underneath.
/// </summary>
public readonly record struct RectangleF
{
    public Vector2 Location { get; init; }
    public Vector2 Size { get; init; }
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

    public Rectangle ToRectangle()
    {
        return new(Vector2.Round(Location).ToPoint(), Vector2.Round(Size).ToPoint());
    }

    public bool Contains(Vector2 point)
    {
        return point.X > Left && point.X < Right && point.Y > Top && point.Y < Bottom;
    }

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

    public RectangleF GetMinkowskiDifference(RectangleF other)
    {
        return new(
            Left - other.Right,
            Top - other.Bottom,
            Width + other.Width,
            Height + other.Height
        );
    }

    /// <summary>
    /// Assuming this rectangle is the Minkowski difference of rectangle A and rectangle B, returns
    /// the extent to which A is penetrating B. The return value is meaningless if this rectangle
    /// does not contain the origin.
    /// </summary>
    public Vector2 GetMinkowskiPenetrationVector()
    {
        var minDist = -Left;
        var penetration = new Vector2(Left, 0);
        if (Right < minDist)
        {
            minDist = Right;
            penetration = new Vector2(Right, 0);
        }
        if (-Top < minDist)
        {
            minDist = -Top;
            penetration = new Vector2(0, Top);
        }
        if (Bottom < minDist)
        {
            penetration = new Vector2(0, Bottom);
        }

        return penetration;
    }
}
