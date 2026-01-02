using Engine;
using Microsoft.Xna.Framework;

namespace Library;

/// <summary>
/// Container for all player state.
/// </summary>
public class Player : Component
{
    public const int PhysicsColliderIndex = 10;
    public const int GroundCheckColliderIndex = 11;
    private static readonly Vector2 PhysicsSize = new(8, 16);

    public bool IsGrounded { get; set; } = false;

    public static Entity MakeEntity(Scene scene)
    {
        return scene
            .StageCreate(nameof(Player))
            .StageAttach(new Player())
            .StageAttach(new PlayerInputController())
            .StageAttach(new PlayerPhysicsController())
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
