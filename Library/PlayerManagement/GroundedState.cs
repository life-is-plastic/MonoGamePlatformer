using Engine;
using Microsoft.Xna.Framework.Input;

namespace Library.PlayerManagement;

public class GroundedState : State
{
    public LateralMotionHelper LateralMotionHelper { get; init; } = new();

    public override Trigger Update()
    {
        var inputManager = Player.Scene.Singletons.Get<InputManager>();
        var velocity = Player.Entity.Get<Velocity>();

        velocity.Linear.X = LateralMotionHelper.NextVelocity(
            velocity.Linear.X,
            GetLateralInput(inputManager),
            Scene.DeltaTime
        );

        if (inputManager.IsPressed(Keys.Space))
        {
            return Trigger.Jump;
        }
        return Trigger.None;
    }
}
