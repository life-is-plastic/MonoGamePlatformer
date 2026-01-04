using Engine;
using Microsoft.Xna.Framework.Input;

namespace Library.PlayerManagement;

public abstract class State
{
    public required Player Player { get; init; }
    public Scene Scene => Player.Scene;
    public LateralMotionHelper LateralMotionHelper = new();

    public abstract Trigger Update();

    protected void MoveLaterally()
    {
        var inputManager = Player.Scene.Singletons.Get<InputManager>();
        var normalizedInput = 0f;
        if (inputManager.IsDown(Keys.A))
        {
            normalizedInput -= 1;
        }
        if (inputManager.IsDown(Keys.D))
        {
            normalizedInput += 1;
        }

        var velocity = Player.Entity.Get<Velocity>();
        velocity.Linear.X = LateralMotionHelper.NextVelocity(
            velocity.Linear.X,
            normalizedInput,
            Scene.DeltaTime
        );
    }
}
