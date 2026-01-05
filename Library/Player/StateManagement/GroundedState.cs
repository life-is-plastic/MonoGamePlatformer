using Engine;
using Microsoft.Xna.Framework.Input;

namespace Library.Player;

public class GroundedState : State
{
    public LateralHelper LateralHelper { get; init; } = new();

    public override Trigger Update()
    {
        var inputManager = Player.Scene.Singletons.Get<InputManager>();
        var velocity = Player.Entity.Get<Velocity>();

        velocity.Linear.X = LateralHelper.NextVelocity(
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
