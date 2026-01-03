using Engine;
using Microsoft.Xna.Framework.Input;
using Stateless;

namespace Library.Player;

public class AirborneState : State
{
    private float _heldJumpEndTime;

    public float Gravity { get; init; } = 800;
    public float HeldJumpGravity { get; init; } = 400;
    public float MaxHeldJumpTime { get; init; } = 0.4f;
    public float JumpSpeed { get; init; } = 250;
    public bool IsHeldJump => _heldJumpEndTime > float.NegativeInfinity;

    public AirborneState(Main player)
        : base(player) { }

    public void OnEntry(StateMachine<State, StateTrigger>.Transition t)
    {
        _heldJumpEndTime = float.NegativeInfinity;
        if (t.Trigger == StateTrigger.Jump)
        {
            _heldJumpEndTime = Player.Scene.CurrentTime + MaxHeldJumpTime;
            var velocity = Player.Entity.Get<Velocity>();
            velocity.Linear.Y = -JumpSpeed;
        }
    }

    public override StateTrigger Update()
    {
        MoveLaterally();

        var inputManager = Player.Scene.Singletons.Get<InputManager>();
        var velocity = Player.Entity.Get<Velocity>();

        var effectiveGravity = Gravity;
        if (Player.Scene.CurrentTime < _heldJumpEndTime && inputManager.IsDown(Keys.Space))
        {
            effectiveGravity = HeldJumpGravity;
        }
        else
        {
            _heldJumpEndTime = float.NegativeInfinity;
        }

        velocity.Linear.Y += effectiveGravity * Player.Scene.DeltaTime;
        return StateTrigger.None;
    }
}
