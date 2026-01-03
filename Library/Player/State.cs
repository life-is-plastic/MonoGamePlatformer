using Engine;
using Microsoft.Xna.Framework.Input;

namespace Library.Player;

public abstract class State
{
    protected Main Player { get; }

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
            velocity.Linear.X = -Player.LateralSpeed;
        }
        else if (inputManager.IsDown(Keys.D))
        {
            velocity.Linear.X = Player.LateralSpeed;
        }
    }
}
