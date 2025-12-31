using Engine.Core;

namespace Engine.Physics;

/// <summary>
/// Pushes other colliders away to eliminate overlaps.
/// </summary>
public partial class StaticGeometryResolver : Component { }

public partial class StaticGeometryResolver : ICollisionHandler
{
    void ICollisionHandler.OnCollisionEnter(in ContactInfo contact)
    {
        ICollisionHandler handler = this;
        handler.OnCollisionStay(contact);
    }

    void ICollisionHandler.OnCollisionStay(in ContactInfo contact)
    {
        var otherTransform = contact.Other.Entity.Get<Transform>();
        otherTransform.Position += contact.Penetration;
    }
}
