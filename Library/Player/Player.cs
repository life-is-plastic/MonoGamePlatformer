using Engine;
using Microsoft.Xna.Framework;

namespace Library;

/// <summary>
/// Marker component for the player entity.
/// </summary>
public class Player : Component
{
    public const int PhysicsColliderIndex = 10;
    public const int GroundCheckColliderIndex = 11;

    public static Entity MakeEntity(Scene scene)
    {
        var size = new Vector2(8, 16);

        return scene
            .StageCreate(nameof(Player))
            .StageAttach(new Player())
            .StageAttach(new PlayerController())
            .StageAttach(new Transform() { Position = new(50, 50) })
            .StageAttach(new Velocity())
            .StageAttach(
                new Collider(size)
                {
                    NormalizedOrigin = new(0.5f),
                    ComponentIndex = PhysicsColliderIndex,
                }
            )
            .StageAttach(
                new Collider(size.X - 2, 1)
                {
                    Origin = new((size.X - 2) / 2, -size.Y / 2),
                    ComponentIndex = GroundCheckColliderIndex,
                }
            )
            .StageAttach(
                new RectangleRenderer()
                {
                    Size = size,
                    NormalizedOrigin = new(0.5f),
                    Color = Color.DarkSlateGray,
                }
            );
    }
}
