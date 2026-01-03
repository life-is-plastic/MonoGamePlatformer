using Engine;
using Microsoft.Xna.Framework.Input;

namespace Library.Player;

public class GroundedState : State
{
    public GroundedState(Main player)
        : base(player) { }

    public override StateTrigger Update()
    {
        MoveLaterally();

        var inputManager = Player.Scene.Singletons.Get<InputManager>();
        if (inputManager.IsPressed(Keys.Space))
        {
            return StateTrigger.Jump;
        }
        return StateTrigger.None;
    }
}
