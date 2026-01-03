using Engine;
using Microsoft.Xna.Framework;

namespace Library.Player;

public class DebugVisualization : Component, IUpdatable
{
    int IUpdatable.UpdateOrder => IUpdatable.UpdateOrderFrameEnd;

    void IUpdatable.Update()
    {
        var player = Entity.Get<Main>();
        var renderer = Entity.Get<RectangleRenderer>();
        if (player.StateMachine.State == player.GroundedState)
        {
            renderer.Color = Color.DarkSlateGray;
        }
        else if (player.StateMachine.State == player.AirborneState)
        {
            if (player.AirborneState.IsHeldJump)
            {
                renderer.Color = Color.Red;
            }
            else
            {
                renderer.Color = Color.DarkGray;
            }
        }
        //
    }
}
