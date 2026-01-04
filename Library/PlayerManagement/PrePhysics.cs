using Engine;

namespace Library.PlayerManagement;

public class PrePhysics : Component, IUpdatable
{
    int IUpdatable.UpdateOrder => 0;

    void IUpdatable.Update()
    {
        var player = Entity.Get<Player>();
        if (player.ProposedTrigger != Trigger.None)
        {
            player.StateMachine.Fire(player.ProposedTrigger);
        }
        player.ProposedTrigger = player.StateMachine.State.Update();
    }
}
