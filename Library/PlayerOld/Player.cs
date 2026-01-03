using Engine;
using Microsoft.Xna.Framework;

namespace Library;

/// <summary>
/// Container for all player state.
/// </summary>
public class PlayerOld : Component
{
    private static readonly Vector2 PhysicsSize = new(8, 16);

    public static int PhysicsColliderIndex => 10;
    public static int GroundCheckColliderIndex => 11;

    public bool IsGrounded { get; set; } = false;
    public bool IsJetpacking { get; set; } = false;

    public static Entity MakeEntity(Scene scene)
    {
        return scene
            .StageCreate(nameof(PlayerOld))
            .StageAttach(new PlayerOld())
            .StageAttach(new PlayerPrePhysics())
            .StageAttach(new PlayerPostPhysics())
            .StageAttach(new PlayerMisc())
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
            );
    }
}
