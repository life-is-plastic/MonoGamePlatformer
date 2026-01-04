using Engine;
using Microsoft.Xna.Framework.Input;

namespace Library.Player;

public abstract class State
{
    public required Main Player { get; init; }
    public Scene Scene => Player.Scene;
    public float LateralSpeed { get; init; } = 100;

    public abstract Trigger Update();

    protected void MoveLaterally()
    {
        var inputManager = Player.Scene.Singletons.Get<InputManager>();
        var velocity = Player.Entity.Get<Velocity>();
        if (!(inputManager.IsDown(Keys.A) ^ inputManager.IsDown(Keys.D)))
        {
            velocity.Linear.X = 0;
        }
        else if (inputManager.IsDown(Keys.A))
        {
            velocity.Linear.X = -LateralSpeed;
        }
        else if (inputManager.IsDown(Keys.D))
        {
            velocity.Linear.X = LateralSpeed;
        }
    }
}
