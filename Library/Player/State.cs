using Engine;
using Microsoft.Xna.Framework.Input;

namespace Library.Player;

public abstract class State
{
    public float LateralSpeed { get; init; } = 100;

    protected Main Player { get; }
    protected Scene Scene => Player.Scene;

    protected State(Main player)
    {
        Player = player;
    }

    public abstract StateTrigger Update();

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
