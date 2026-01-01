using System;
using Engine.Core;
using Microsoft.Xna.Framework;

namespace Engine.Physics;

/// <summary>
/// Pushes other colliders away to eliminate overlaps.
/// </summary>
public partial class StaticGeometryResolver : Component
{
    private static void PushOther(in ContactInfo contact)
    {
        var transform = contact.Other.Entity.Get<Transform>();
        transform.Position += contact.Penetration;

        if (
            contact.Other.Entity.MaybeGet<Velocity>() is { } velocity
            && Vector2.Dot(velocity.Linear, contact.Penetration) > 0
        )
        {
            velocity.Linear -= Vector2.Normalize(contact.Penetration) * velocity.Linear;
        }
    }
}

public partial class StaticGeometryResolver : ICollisionHandler
{
    void ICollisionHandler.OnCollisionEnter(in ContactInfo contact)
    {
        PushOther(contact);
    }

    void ICollisionHandler.OnCollisionStay(in ContactInfo contact)
    {
        PushOther(contact);
    }
}
