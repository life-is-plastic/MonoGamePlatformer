using Engine;
using Microsoft.Xna.Framework.Input;
using Stateless;

namespace Library.PlayerManagement;

public class AirborneState : State
{
    private float _heldJumpEndTime;

    public LateralMotionHelper LateralMotionHelper { get; init; } = new();
    public JumpMotionHelper JumpMotionHelper { get; init; } = new();

    public float HeldJumpGravity { get; init; } = 300;
    public float MaxHeldJumpTime { get; init; } = 0.4f;
    public bool IsHeldJump => _heldJumpEndTime > float.NegativeInfinity;

    public void OnEntry(StateMachine<State, Trigger>.Transition t)
    {
        _heldJumpEndTime = float.NegativeInfinity;
        if (t.Trigger == Trigger.Jump)
        {
            _heldJumpEndTime = Player.Scene.CurrentTime + MaxHeldJumpTime;
            var velocity = Player.Entity.Get<Velocity>();
            velocity.Linear.Y =
                JumpMotionHelper.InitialVelocity
                - JumpMotionHelper.Gravity * Player.Scene.DeltaTime;
        }
    }

    public override Trigger Update()
    {
        var inputManager = Player.Scene.Singletons.Get<InputManager>();
        var velocity = Player.Entity.Get<Velocity>();

        velocity.Linear.X = LateralMotionHelper.NextVelocity(
            velocity.Linear.X,
            GetLateralInput(inputManager),
            Scene.DeltaTime
        );

        var effectiveGravity = JumpMotionHelper.Gravity;
        if (Player.Scene.CurrentTime < _heldJumpEndTime && inputManager.IsDown(Keys.Space))
        {
            effectiveGravity = HeldJumpGravity;
        }
        else
        {
            _heldJumpEndTime = float.NegativeInfinity;
        }

        velocity.Linear.Y += effectiveGravity * Player.Scene.DeltaTime;
        return Trigger.None;
    }
}
