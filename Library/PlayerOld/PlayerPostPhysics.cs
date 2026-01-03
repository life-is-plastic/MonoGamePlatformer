using System;
using System.Collections.Generic;
using Engine;
using Microsoft.Xna.Framework;

namespace Library;

/// <summary>
/// Handles responding to collisions.
/// </summary>
public class PlayerPostPhysics : Component, IUpdatable, ICollisionHandler
{
    private readonly List<Collider> _groundColliderOverlaps = new();

    int IUpdatable.UpdateOrder => IUpdatable.UpdateOrderPhysics[^1] + 1;

    void IUpdatable.Update()
    {
        var player = Entity.Get<PlayerOld>();
        var groundCollider = Entity.Get<Collider>(PlayerOld.GroundCheckColliderIndex);
        var groundColliderRect = groundCollider.AsWorldRectangleF();

        player.IsGrounded = false;
        foreach (var collider in _groundColliderOverlaps)
        {
            var rect = collider.AsWorldRectangleF();
            if (
                groundColliderRect.GetOverlap(rect) is { } overlap
                && Math.Abs(overlap.Top - rect.Top) < 0.05f
            )
            {
                player.IsGrounded = true;
                break;
            }
        }
        if (player.IsGrounded)
        {
            player.IsJetpacking = false;
        }
        _groundColliderOverlaps.Clear();
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

        if (contact.Mine.ComponentIndex == PlayerOld.PhysicsColliderIndex)
        {
            var transform = Entity.Get<Transform>();
            var velocity = Entity.Get<Velocity>();
            transform.Position -= contact.Penetration;
            if (contact.Penetration.Y != 0 && Vector2.Dot(contact.Penetration, velocity.Linear) > 0)
            {
                velocity.Linear.Y = 0;
                var player = Entity.Get<PlayerOld>();
                player.IsJetpacking = false;
            }
            return;
        }

        if (contact.Mine.ComponentIndex == PlayerOld.GroundCheckColliderIndex)
        {
            _groundColliderOverlaps.Add(contact.Other);
            return;
        }
    }

    void ICollisionHandler.OnCollisionExit(in ContactInfo contact)
    {
        if (!contact.Other.Entity.Has<StaticGeometry>())
        {
            return;
        }

        if (contact.Mine.ComponentIndex == PlayerOld.GroundCheckColliderIndex)
        {
            return;
        }
    }
}
