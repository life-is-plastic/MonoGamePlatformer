using Engine;
using Microsoft.Xna.Framework;

namespace Library;

public class PlayerMisc : Component, IUpdatable
{
    int IUpdatable.UpdateOrder => IUpdatable.UpdateOrderFrameEnd;

    void IUpdatable.Update()
    {
        var player = Entity.Get<Player>();
        var renderer = Entity.Get<RectangleRenderer>();
        if (player.IsGrounded)
        {
            renderer.Color = Color.DarkSlateGray;
        }
        else if (player.IsJetpacking)
        {
            renderer.Color = Color.Red;
        }
        else
        {
            renderer.Color = Color.Gray;
        }
    }
}
