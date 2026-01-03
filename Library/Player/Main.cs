using System;
using Engine;
using Microsoft.Xna.Framework;
using Stateless;

namespace Library.Player;

/// <summary>
/// The central player component.
/// </summary>
public class Main : Component
{
    private static readonly Action s_emptyAction = () => { };

    public static int PhysicsColliderIndex => 10;
    public static int GroundCheckColliderIndex => 11;
    public static Vector2 PhysicsSize => new(8, 16);

    public StateMachine<State, StateTrigger> StateMachine;
    public GroundedState GroundedState { get; }
    public AirborneState AirborneState { get; }
    public StateTrigger ProposedTrigger { get; set; } = StateTrigger.None;

    public Main()
    {
        GroundedState = new(this);
        AirborneState = new(this);

        StateMachine = new(AirborneState);
        StateMachine
            .Configure(GroundedState)
            .Permit(StateTrigger.Jump, AirborneState)
            .Permit(StateTrigger.EnsureUngrounded, AirborneState)
            .InternalTransition(StateTrigger.EnsureGrounded, s_emptyAction);
        StateMachine
            .Configure(AirborneState)
            .OnEntry(AirborneState.OnEntry)
            .Permit(StateTrigger.EnsureGrounded, GroundedState)
            .InternalTransition(StateTrigger.EnsureUngrounded, s_emptyAction);
        ;
    }

    public static Entity MakeEntity(Scene scene)
    {
        return scene
            .StageCreate(nameof(Player))
            .StageAttach(new Main())
            .StageAttach(new PrePhysics())
            .StageAttach(new PostPhysics())
            .StageAttach(new CameraFollow())
            .StageAttach(new Transform() { Position = new(50, 50) })
            .StageAttach(new Velocity())
            .StageAttach(
                new Collider(PhysicsSize)
                {
                    NormalizedOrigin = new(0.5f),
                    ComponentIndex = PhysicsColliderIndex,
                }
            )
            .StageAttach(
                new Collider(PhysicsSize.X, 1)
                {
                    Origin = new(PhysicsSize.X / 2, -PhysicsSize.Y / 2),
                    ComponentIndex = GroundCheckColliderIndex,
                }
            )
            .StageAttach(
                new RectangleRenderer()
                {
                    Size = PhysicsSize,
                    NormalizedOrigin = new(0.5f),
                    Color = Color.DarkSlateGray,
                }
            )
            .StageAttach(new DebugVisualization());
    }
}
