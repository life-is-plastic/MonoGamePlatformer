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
    public Collider Mine { get; init; }

    /// <summary>
    /// The other collider involved.
    /// </summary>
    public Collider Other { get; init; }

    /// <summary>
    /// The Minkowski difference of <c>Mine</c> - <c>Other</c>.
    /// </summary>
    public RectangleF MinkowskiDifference { get; init; }

    /// <summary>
    /// To what extent <c>Mine</c> is penetrating <c>Other</c>. Subtracting this value from
    /// <c>Mine</c>'s position will push <c>Mine</c> fully out of <c>Other</c>.
    /// </summary>
    public Vector2 Penetration { get; init; }

    /// <summary>
    /// Returns a new contact info presenting data from the perspective of <c>Other</c>.
    /// </summary>
    public ContactInfo Inverted()
    {
        return new()
        {
            Mine = Other,
            Other = Mine,
            MinkowskiDifference = MinkowskiDifference.WithCenter(-MinkowskiDifference.Center),
            Penetration = -Penetration,
        };
    }
}
