using Engine.Util;
using Microsoft.Xna.Framework;

namespace Engine.Physics;

/// <summary>
/// Information about the contact between two colliders in a particular frame. As a collision
/// handler argument, the data is always presented from the perspective of the collision handler's
/// entity.
/// </summary>
public readonly struct ContactInfo
{
    /// <summary>
    /// The "self" collider. When a collision handler receives a contact info, this property points
    /// to the collision handler's sibling collider (as opposed to <c>Other</c> which belongs to a
    /// different entity).
    /// </summary>
    public Collider Mine { get; }

    /// <summary>
    /// The other collider involved.
    /// </summary>
    public Collider Other { get; }

    /// <summary>
    /// The overlapping world-space rectangle between the two colliders.
    /// </summary>
    public RectangleF Overlap { get; }

    /// <summary>
    /// A directional unit vector corresponding to the <c>Other</c> edge that <c>Mine</c> has
    /// collided into.
    /// </summary>
    public Vector2 Normal { get; }

    public ContactInfo(Collider mine, Collider other, RectangleF overlap, Vector2 normal)
    {
        Mine = mine;
        Other = other;
        Overlap = overlap;
        Normal = normal;
    }

    /// <summary>
    /// Returns a new contact info presenting data from the perspective of <c>Other</c>.
    /// </summary>
    public ContactInfo Inverted()
    {
        return new(Other, Mine, Overlap, -Normal);
    }
}
