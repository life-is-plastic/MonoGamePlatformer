using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.Debugging;

/// <summary>
/// Spawn rectangles and see how they interact.
/// </summary>
public class StaticGeometryScene : BaseSceneDefinition
{
    public static StaticGeometryScene Instance { get; } = new();

    private StaticGeometryScene() { }

    public override void Initialize(Scene scene)
    {
        base.Initialize(scene);
        scene.Singletons.StageAttach(new Controller());
    }

    private class Controller : Component, IUpdatable
    {
        private static readonly Vector2 s_dims = new(32, 32);

        void IUpdatable.Update()
        {
            var inputManager = Scene.Singletons.Get<InputManager>();
            if (inputManager.IsPressed(Keys.D1))
            {
                Scene
                    .StageCreate("Object")
                    .StageAttach(new Transform() { Position = inputManager.MouseWorldPosition })
                    .StageAttach(new Collider(s_dims) { NormalizedOrigin = new(0.5f) })
                    .StageAttach(
                        new RectangleRenderer()
                        {
                            Size = s_dims,
                            NormalizedOrigin = new(0.5f),
                            Color = Color.SaddleBrown,
                        }
                    );
            }
            if (inputManager.IsPressed(Keys.D2))
            {
                Scene
                    .StageCreate("Object")
                    .StageAttach(new Transform() { Position = inputManager.MouseWorldPosition })
                    .StageAttach(new StaticGeometryResolver())
                    .StageAttach(
                        new Collider(s_dims)
                        {
                            NormalizedOrigin = new(0.5f),
                            Layer = CollisionLayers.StaticGeometry,
                        }
                    )
                    .StageAttach(
                        new RectangleRenderer()
                        {
                            Size = s_dims,
                            NormalizedOrigin = new(0.5f),
                            Color = Color.Black,
                        }
                    );
            }
        }
    }
}
