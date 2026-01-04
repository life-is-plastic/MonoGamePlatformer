using System;
using Engine;
using Engine.Util;
using Microsoft.Xna.Framework;
using Stateless;

namespace Library.PlayerManagement;

/// <summary>
/// The central player component.
/// </summary>
public class Player : Component
{
    private static readonly Action s_emptyAction = () => { };

    public static int PhysicsColliderIndex => 10;
    public static int GroundCheckColliderIndex => 11;
    public static Vector2 PhysicsSize => new(8, 16);

    public StateMachine<State, Trigger> StateMachine;
    public GroundedState GroundedState { get; }
    public AirborneState AirborneState { get; }
    public Trigger ProposedTrigger { get; set; } = Trigger.None;

    public Player()
    {
        GroundedState = new() { Player = this };
        AirborneState = new() { Player = this };

        StateMachine = new(AirborneState);
        StateMachine
            .Configure(GroundedState)
            .Permit(Trigger.Jump, AirborneState)
            .Permit(Trigger.EnsureUngrounded, AirborneState)
            .InternalTransition(Trigger.EnsureGrounded, s_emptyAction);
        StateMachine
            .Configure(AirborneState)
            .OnEntry(AirborneState.OnEntry)
            .Permit(Trigger.EnsureGrounded, GroundedState)
            .InternalTransition(Trigger.EnsureUngrounded, s_emptyAction);
        ;
    }

    public static Entity MakeEntity(Scene scene)
    {
        return scene
            .StageCreate(nameof(PlayerManagement))
            .StageAttach(new Player())
            .StageAttach(new PrePhysics())
            .StageAttach(new PostPhysics())
            .StageAttach(new CameraFollow())
            .StageAttach(new Transform() { Position = new(50, 50) })
            .StageAttach(new Velocity())
            .StageAttach(
                new Collider()
                {
                    Size = PhysicsSize,
                    NormalizedOrigin = new(0.5f),
                    ComponentIndex = PhysicsColliderIndex,
                }
            )
            .StageAttach(
                new Collider()
                {
                    Size = PhysicsSize.WithY(1),
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
