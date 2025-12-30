using Engine.Core;
using Engine.Graphics;
using Engine.Input;
using Engine.Physics;
using Engine.ZDebug.Inputs;
using Engine.ZDebug.Visuals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.ZDebug.Scenes;

public class PhysicsSandboxScene : ISceneDefinition
{
    public static PhysicsSandboxScene Instance { get; } = new();

    private PhysicsSandboxScene() { }

    string ISceneDefinition.Name()
    {
        return nameof(PhysicsSandboxScene);
    }

    void ISceneDefinition.Initialize(Scene scene)
    {
        scene
            .Singletons.StageAttach(new Controller())
            .StageAttach(new SceneLoadOnPress(Instance))
            .StageAttach(new ScenePauseToggle())
            .StageAttach(new CameraMouseZoom())
            .StageAttach(new CameraMouseDrag())
            .StageAttach(new ColliderMouseDrag(entity => true))
            .StageAttach(new ColliderRenderer(Keys.P))
            .StageAttach(new DrawHelper());
    }

    private class Controller : Component, IUpdatable
    {
        void IUpdatable.Update()
        {
            var inputManager = Scene.Singletons.Get<InputManager>();
            if (inputManager.IsPressed(Keys.D1))
            {
                Scene
                    .StageCreate("Object")
                    .StageAttach(new Transform() { Position = inputManager.MouseWorldPosition })
                    .StageAttach(new Collider(32, 32) { NormalizedOrigin = new(0.5f, 0.5f) })
                    .StageAttach(
                        new RectangleRenderer()
                        {
                            Size = new(32, 32),
                            NormalizedOrigin = new(0.5f, 0.5f),
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
                        new Collider(32, 32)
                        {
                            NormalizedOrigin = new(0.5f, 0.5f),
                            Layer = CollisionLayers.StaticGeometry,
                        }
                    )
                    .StageAttach(
                        new RectangleRenderer()
                        {
                            Size = new(32, 32),
                            NormalizedOrigin = new(0.5f, 0.5f),
                            Color = Color.Black,
                        }
                    );
            }
        }
    }
}
