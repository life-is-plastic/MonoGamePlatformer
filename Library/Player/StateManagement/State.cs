using Engine;
using Microsoft.Xna.Framework.Input;

namespace Library.Player;

public abstract class State
{
    public required PlayerComponent Player { get; init; }
    protected Scene Scene => Player.Scene;

    public abstract Trigger Update();

    protected static int GetLateralInput(InputManager inputManager)
    {
        var input = 0;
        if (inputManager.IsDown(Keys.A))
        {
            input -= 1;
        }
        if (inputManager.IsDown(Keys.D))
        {
            input += 1;
        }
        return input;
    }
}
