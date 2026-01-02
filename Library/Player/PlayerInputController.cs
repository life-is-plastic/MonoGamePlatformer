using Engine;
using Microsoft.Xna.Framework.Input;

namespace Library;

public class PlayerInputController : Component, IUpdatable
{
    private const float Gravity = 500;
    private const float JumpSpeed = 200;
    private const float LateralSpeed = 100;
    private InputManager _inputManager = null!;
    private Player _player = null!;
    private Velocity _velocity = null!;

    private void BeginUpdate()
    {
        _inputManager = Scene.Singletons.Get<InputManager>();
        _player = Entity.Get<Player>();
        _velocity = Entity.Get<Velocity>();
    }

    private void ApplyGravity()
    {
        if (_player.IsGrounded)
        {
            return;
        }
        _velocity.Linear.Y += Gravity * Scene.DeltaTime;
    }

    private void Jump()
    {
        if (!_player.IsGrounded)
        {
            return;
        }
        if (!_inputManager.IsPressed(Keys.Space))
        {
            return;
        }
        _velocity.Linear.Y = -JumpSpeed;
    }

    private void MoveLaterally()
    {
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

    void IUpdatable.Update()
    {
        BeginUpdate();
        ApplyGravity();
        Jump();
        MoveLaterally();
    }
}
