using Engine;
using Microsoft.Xna.Framework.Input;
using Stateless;

namespace Library.PlayerManagement;

public class AirborneState : State
{
    public LateralMotionHelper LateralMotionHelper { get; init; } = new();
    public JumpHelper JumpHelper { get; init; } = JumpHelper.Default;
    public JumpHelper HeldInputJumpHelper { get; init; } = JumpHelper.HeldInput;

    public float HeldJumpGravity { get; init; } = 300;
    public float MaxHeldJumpTime { get; init; } = 0.4f;
    public bool IsHeldJump { get; private set; } = false;

    public void OnEntry(StateMachine<State, Trigger>.Transition t)
    {
        if (t.Trigger == Trigger.Jump)
        {
            IsHeldJump = true;
            var velocity = Player.Entity.Get<Velocity>();
            velocity.Linear.Y = JumpHelper.InitialVelocity;
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

        var gravity = JumpHelper.Gravity;
        if (IsHeldJump)
        {
            if (inputManager.IsDown(Keys.Space) && velocity.Linear.Y < 0)
            {
                gravity = HeldInputJumpHelper.Gravity;
            }
            else
            {
                IsHeldJump = false;
            }
        }

        velocity.Linear.Y += gravity * Player.Scene.DeltaTime;
        return Trigger.None;
    }
}
