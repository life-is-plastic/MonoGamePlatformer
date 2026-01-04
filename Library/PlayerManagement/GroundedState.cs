using Engine;
using Microsoft.Xna.Framework.Input;

namespace Library.PlayerManagement;

public class GroundedState : State
{
    public override Trigger Update()
    {
        MoveLaterally();

        var inputManager = Player.Scene.Singletons.Get<InputManager>();
        if (inputManager.IsPressed(Keys.Space))
        {
            return Trigger.Jump;
        }
        return Trigger.None;
    }
}
