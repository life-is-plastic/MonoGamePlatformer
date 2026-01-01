using System;
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Library;

public partial class PlayerController : Component
{
    public const float Gravity = 500;
    public const float JumpSpeed = 200;
    public const float LateralSpeed = 100;

    private bool _isGrounded = false;
    private InputManager _inputManager = null!;
    private Transform _transform = null!;
    private Velocity _velocity = null!;

    private void BeginUpdate()
    {
        _inputManager = Scene.Singletons.Get<InputManager>();
        _transform = Entity.Get<Transform>();
        _velocity = Entity.Get<Velocity>();
    }

    private void ApplyGravity()
    {
        if (_isGrounded)
        {
            return;
        }
        _velocity.Linear.Y += Gravity * Scene.DeltaTime;
    }

    private void Jump()
    {
        if (!_isGrounded)
        {
            return;
        }
        if (!_inputManager.IsPressed(Keys.Space))
        {
            return;
        }
        _velocity.Linear.Y = -JumpSpeed;
        // _isGrounded = false;
    }

    private void HandleStaticGeometryCollision(in ContactInfo contact)
    {
        if (!contact.Other.Entity.Has<StaticGeometry>())
        {
            return;
        }

        if (contact.Mine.ComponentIndex == Player.GroundCheckColliderIndex)
        {
            _isGrounded = true;
        }
        else
        {
            _transform.Position -= contact.Penetration;
            if (
                contact.Penetration.Y != 0
                && Vector2.Dot(contact.Penetration, _velocity.Linear) > 0
            )
            {
                Console.Out.WriteLine($"{contact.Penetration} {_velocity.Linear} {_isGrounded}");
                _velocity.Linear.Y = 0;
            }
        }
    }
}

public partial class PlayerController : IUpdatable
{
    void IUpdatable.Update()
    {
        BeginUpdate();

        ApplyGravity();
        Jump();

        if (!(_inputManager.IsDown(Keys.A) ^ _inputManager.IsDown(Keys.D)))
        {
            _velocity.Linear.X = 0;
        }
        else if (_inputManager.IsDown(Keys.A))
        {
            _velocity.Linear.X = -LateralSpeed;
        }
        else if (_inputManager.IsDown(Keys.D))
        {
            _velocity.Linear.X = LateralSpeed;
        }
    }
}

public partial class PlayerController : ICollisionHandler
{
    void ICollisionHandler.OnCollisionEnter(in ContactInfo contact)
    {
        HandleStaticGeometryCollision(contact);
    }

    void ICollisionHandler.OnCollisionStay(in ContactInfo contact)
    {
        HandleStaticGeometryCollision(contact);
    }

    void ICollisionHandler.OnCollisionExit(in ContactInfo contact)
    {
        if (!contact.Other.Entity.Has<StaticGeometry>())
        {
            return;
        }
        if (contact.Mine.ComponentIndex == Player.GroundCheckColliderIndex)
        {
            _isGrounded = false;
        }
    }
}
