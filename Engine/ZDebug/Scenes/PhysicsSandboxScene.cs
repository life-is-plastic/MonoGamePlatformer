using Engine.Core;
using Engine.Graphics;
using Engine.Input;
using Engine.Physics;
using Engine.ZDebug.Inputs;
using Engine.ZDebug.Visuals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            .StageAttach(new OriginRenderer())
            .StageAttach(new MinkowskiViz());
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

internal class MinkowskiViz : Component, IUpdatable
{
    private Collider _col1 = null!;
    private Collider _col2 = null!;
    private Collider _col3 = null!;
    private Collider _col4 = null!;

    private Collider MakeCol(int num)
    {
        var collider = new Collider(32, 32) { NormalizedOrigin = new(0.5f, 0.5f) };
        Scene
            .StageCreate("Object")
            .StageAttach(new Transform())
            .StageAttach(
                new DrawableRenderer<SpriteText>(
                    new SpriteText(Scene.Content.Load<SpriteFont>("Fonts/04B_30"))
                    {
                        Message = $"{num}",
                        NormalizedOrigin = new(0.5f, 0.5f),
                    }
                )
                {
                    DrawOrder = 100,
                }
            )
            .StageAttach(collider)
            .StageAttach(
                new RectangleRenderer()
                {
                    Size = new(32, 32),
                    NormalizedOrigin = new(0.5f, 0.5f),
                    Color = Color.SaddleBrown * 0.3f,
                }
            );
        return collider;
    }

    void IUpdatable.Update()
    {
        if (_col1 is null)
        {
            _col1 = MakeCol(1);
            _col2 = MakeCol(2);
            _col3 = MakeCol(3);
            _col4 = MakeCol(4);
            return;
        }

        var md = _col1.AsWorldRectangleF().GetMinkowskiDifference(_col2.AsWorldRectangleF());
        _col3.Entity.Get<Transform>().Position = md.Center;
        _col3.Entity.Get<RectangleRenderer>().Size = md.Size;
        _col3.Entity.Get<RectangleRenderer>().NormalizedOrigin = new(0.5f, 0.5f);

        // md = _col2.AsWorldRectangleF().MinkowskiDifference(_col1.AsWorldRectangleF());
        _col4.Entity.Get<Transform>().Position = -md.Center;
        _col4.Entity.Get<RectangleRenderer>().Size = md.Size;
        _col4.Entity.Get<RectangleRenderer>().NormalizedOrigin = new(0.5f, 0.5f);
    }
}
