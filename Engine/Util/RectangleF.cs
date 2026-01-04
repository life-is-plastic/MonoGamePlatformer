using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Engine.Util;

/// <summary>
/// Like <c>Microsoft.Xna.Framework.Rectangle</c> but stores floats underneath.
/// </summary>
public readonly record struct RectangleF
{
    public Vector2 Location { get; init; } = new();
    public Vector2 Size
    {
        get;
        init
        {
            Debug.Assert(value.X >= 0 && value.Y >= 0);
            field = value;
        }
    } = new();

    public Vector2 Center => Location + Size / 2;
    public float X => Location.X;
    public float Y => Location.Y;
    public float Width => Size.X;
    public float Height => Size.Y;
    public float Left => X;
    public float Right => X + Width;
    public float Top => Y;
    public float Bottom => Y + Height;

    public RectangleF() { }

    public RectangleF WithCenter(Vector2 center) =>
        new() { Location = center - Size / 2, Size = Size };

    public RectangleF WithCenter(float? x = null, float? y = null) =>
        WithCenter(new Vector2(x ?? X, y ?? Y));

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
        return new() { Location = new(left, top), Size = new(right - left, bottom - top) };
    }

    public RectangleF GetMinkowskiDifference(RectangleF other)
    {
        return new()
        {
            Location = new(Left - other.Right, Top - other.Bottom),
            Size = new(Width + other.Width, Height + other.Height),
        };
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
