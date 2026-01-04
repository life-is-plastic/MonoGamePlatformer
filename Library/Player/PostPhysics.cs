using System;
using System.Collections.Generic;
using Engine;
using Microsoft.Xna.Framework;

namespace Library.Player;

public class PostPhysics : Component, IUpdatable, ICollisionHandler
{
    private readonly List<Collider> _groundColliderOverlaps = new();

    int IUpdatable.UpdateOrder => IUpdatable.UpdateOrderPhysics[^1] + 1;

    void IUpdatable.Update()
    {
        var groundCollider = Entity.Get<Collider>(Main.GroundCheckColliderIndex);
        var groundColliderRect = groundCollider.AsWorldRectangleF();

        var shouldBeGrounded = false;
        foreach (var collider in _groundColliderOverlaps)
        {
            var rect = collider.AsWorldRectangleF();
            if (
                groundColliderRect.GetOverlap(rect) is { } overlap
                && Math.Abs(overlap.Top - rect.Top) < 0.05f
            )
            {
                shouldBeGrounded = true;
                break;
            }
        }

        var player = Entity.Get<Main>();
        var proposedTrigger = shouldBeGrounded ? Trigger.EnsureGrounded : Trigger.EnsureUngrounded;
        if (proposedTrigger > player.ProposedTrigger)
        {
            player.ProposedTrigger = proposedTrigger;
        }
    }

    void ICollisionHandler.OnCollisionEnter(in ContactInfo contact)
    {
        ICollisionHandler handler = this;
        handler.OnCollisionStay(contact);
    }

    void ICollisionHandler.OnCollisionStay(in ContactInfo contact)
    {
        if (!contact.Other.Entity.Has<StaticGeometry>())
        {
            return;
        }

        if (contact.Mine.ComponentIndex == Main.PhysicsColliderIndex)
        {
            var transform = Entity.Get<Transform>();
            var velocity = Entity.Get<Velocity>();
            transform.Position -= contact.Penetration;
            if (contact.Penetration.Y != 0 && Vector2.Dot(contact.Penetration, velocity.Linear) > 0)
            {
                velocity.Linear.Y = 0;
            }
            return;
        }

        if (contact.Mine.ComponentIndex == Main.GroundCheckColliderIndex)
        {
            _groundColliderOverlaps.Add(contact.Other);
            return;
        }
    }
}
