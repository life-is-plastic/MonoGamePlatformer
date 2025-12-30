using Engine.Core;

namespace Engine.Physics;

public partial class StaticGeometry : Component { }

public partial class StaticGeometry : ICollisionHandler
{
    void ICollisionHandler.OnCollisionEnter(in ContactInfo contact)
    {
        ICollisionHandler handler = this;
        handler.OnCollisionStay(contact);
    }

    void ICollisionHandler.OnCollisionStay(in ContactInfo contact)
    {
        var otherTransform = contact.Other.Entity.Get<Transform>();
        otherTransform.Position -= contact.Normal * contact.Overlap.Size;
    }
}
