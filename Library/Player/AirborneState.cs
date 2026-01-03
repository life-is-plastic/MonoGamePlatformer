using Engine;
using Stateless;

namespace Library.Player;

public class AirborneState : State
{
    public AirborneState(Main player)
        : base(player) { }

    public void OnEntry(StateMachine<State, StateTrigger>.Transition t)
    {
        if (t.Trigger == StateTrigger.Jump)
        {
            var velocity = Player.Entity.Get<Velocity>();
            velocity.Linear.Y = -Player.JumpSpeed;
        }
    }

    public override StateTrigger Update()
    {
        MoveLaterally();
        var velocity = Player.Entity.Get<Velocity>();
        velocity.Linear.Y += Player.Gravity * Player.Scene.DeltaTime;
        return StateTrigger.None;
    }
}
