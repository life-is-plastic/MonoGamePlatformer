using Engine.Core;
using Engine.Physics;

namespace Library.Environment;

public class StaticGeometry : Component, ICollisionHandler
{
    void ICollisionHandler.OnCollisionEnter(in ContactInfo contact)
    {
        ICollisionHandler handler = this;
        handler.OnCollisionStay(contact);
    }

    void ICollisionHandler.OnCollisionStay(in ContactInfo contact)
    {
        if (contact.Other.Entity.Has<StaticGeometry>())
        {
            return;
        }
        var otherTransform = contact.Other.Entity.Get<Transform>();
        otherTransform.Position -= contact.Normal * contact.Overlap.Size;
    }
}
