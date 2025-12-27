using Engine.Core;
using Engine.Util;

namespace Engine.Physics;

/// <summary>
/// AABB rectangle collider. Collision checking ignores collider pairs on the same entity.
/// </summary>
public class Collider : Component
{
    public const int AllLayers = 0;

    /// <summary>
    /// The collider's base rectangular shape, whose center defines a relative offset from the
    /// entity's position assuming scale = 1.
    /// </summary>
    public RectangleF RectangleF { get; }

    public int Layer { get; init; } = AllLayers;

    /// <summary>
    /// If false, then collision checks will ignore this collider.
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    public Collider(RectangleF rect)
    {
        RectangleF = rect;
    }

    /// <summary>
    /// Returns the absolute, world space representation of this collider.
    /// </summary>
    public RectangleF AsWorldRect()
    {
        var transform = Entity.Get<Transform>();
        return RectangleF
            .ScaleFromCenter(transform.Scale)
            .WithCenter(RectangleF.Center * transform.Scale)
            .Translate(transform.Position);
    }

    public RectangleF? GetOverlap(Collider other)
    {
        return AsWorldRect().GetOverlap(other.AsWorldRect());
    }
}
