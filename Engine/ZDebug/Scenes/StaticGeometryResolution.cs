using Engine.Core;
using Engine.Graphics;
using Engine.Input;
using Engine.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.ZDebug.Scenes;

/// <summary>
/// Spawn rectangles and see how they interact.
/// </summary>
public class StaticGeometryResolution : BaseSceneDefinition
{
    public static StaticGeometryResolution Instance { get; } = new();

    private StaticGeometryResolution() { }

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
