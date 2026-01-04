using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.Debugging;

/// <summary>
/// Spawn rectangles and observe them pushing against each other.
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
                    .StageAttach(new Collider() { Size = s_dims, NormalizedOrigin = new(0.5f) })
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
                    .StageAttach(new StaticGeometry())
                    .StageAttach(
                        new Collider()
                        {
                            Size = s_dims,
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

    public class StaticGeometry : Component, ICollisionHandler
    {
        private static void PushOther(in ContactInfo contact)
        {
            var transform = contact.Other.Entity.Get<Transform>();
            transform.Position += contact.Penetration;
        }

        void ICollisionHandler.OnCollisionEnter(in ContactInfo contact)
        {
            PushOther(contact);
        }

        void ICollisionHandler.OnCollisionStay(in ContactInfo contact)
        {
            PushOther(contact);
        }
    }
}
