using System.Diagnostics;
using Engine.Util;
using Microsoft.Xna.Framework;

namespace Engine;

/// <summary>
/// Axis-aligned rectangle collider. Collision checking ignores collider pairs on the same entity.
/// </summary>
public class Collider : Component
{
    public required Vector2 Size
    {
        get;
        init
        {
            Debug.Assert(value.X > 0);
            Debug.Assert(value.Y > 0);
            field = value;
        }
    }
    public float Width => Size.X;
    public float Height => Size.Y;

    /// <summary>
    /// Similar to <c>Sprite.Origin</c>.
    /// </summary>
    public Vector2 Origin { get; init; } = new();

    /// <summary>
    /// Similar to <c>Sprite.NormalizedOrigin</c>.
    /// </summary>
    public Vector2 NormalizedOrigin
    {
        get => Origin / Size;
        init => Origin = value * Size;
    }

    public int Layer
    {
        get;
        init
        {
            Debug.Assert(value >= 0 && value < CollisionLayers.Count);
            field = value;
        }
    } = CollisionLayers.Default;

    /// <summary>
    /// If true, then collision checks will consider this collider.
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Returns the absolute, world space representation of this collider.
    /// </summary>
    public RectangleF AsWorldRectangleF()
    {
        var transform = Entity.Get<Transform>();
        return new RectangleF()
        {
            Location = transform.Position - Origin * transform.Scale,
            Size = Size * transform.Scale,
        };
    }
}
