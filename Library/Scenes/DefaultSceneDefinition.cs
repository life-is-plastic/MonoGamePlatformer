using System;
using Engine.Audio;
using Engine.Core;
using Engine.Graphics;
using Engine.Input;
using Engine.Physics;
using Engine.Util;
using Engine.Util.Debugging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Library.Scenes;

public class DefaultSceneDefinition : ISceneDefinition
{
    public static DefaultSceneDefinition Instance { get; } = new();

    private DefaultSceneDefinition() { }

    string ISceneDefinition.Name()
    {
        return "DefaultScene";
    }

    void ISceneDefinition.Initialize(Scene scene)
    {
        scene
            .Singletons.StageAttach(new SceneLoadOnPress(Instance))
            .StageAttach(new ScenePauseToggle())
            .StageAttach(new CameraMouseDrag())
            .StageAttach(new ColliderRenderer(Keys.P))
            .StageAttach(new DefaultSceneHelper())
            .StageAttach(new DrawHelper());

        DefaultSceneHelper
            .MakeRect(scene, new(0, 60), new(400, 10), Color.SaddleBrown)
            .StageAttach(new StaticGeometry());

        scene
            .StageCreate(nameof(Player))
            .StageAttach(new Player())
            .StageAttach(new Transform())
            .StageAttach(new Velocity())
            .StageAttach(new Collider(20, 20) { NormalizedOrigin = new(0.5f, 0.5f) })
            .StageAttach(new RectangleRenderer() { Size = new(20, 20), Color = Color.DarkGray });

        scene
            .Singletons.Get<AudioManager>()
            .Play(scene.Content.Load<SoundEffect>("Audio/Theme"), loop: true);
    }
}

internal class DefaultSceneHelper : Component, IUpdatable
{
    public static Entity MakeRect(Scene scene, Vector2 position, Vector2 size, Color color)
    {
        return scene
            .StageCreate("Rect")
            .StageAttach(new Transform() { Position = position })
            .StageAttach(new RectangleRenderer() { Size = size, Color = color })
            .StageAttach(new Collider(size) { NormalizedOrigin = new(0.5f, 0.5f) });
    }

    private readonly Random _rng = new();

    bool IUpdatable.Pause()
    {
        return false;
    }

    void IUpdatable.Update()
    {
        var inputManager = Scene.Singletons.Get<InputManager>();
        if (inputManager.IsPressed(Keys.D1))
        {
            var cameraRect = Scene.Find<Camera>().First().Get<Camera>().AsWorldRectangleF();
            MakeRect(
                    Scene,
                    position: new Vector2(
                        MathHelper.Lerp(cameraRect.Left, cameraRect.Right, _rng.NextSingle()),
                        MathHelper.Lerp(cameraRect.Top, cameraRect.Bottom, _rng.NextSingle())
                    ),
                    size: new Vector2(_rng.NextInt64(40, 80), _rng.NextInt64(20, 60)),
                    color: Color.Orange
                )
                .StageAttach(
                    new Velocity
                    {
                        Angular =
                            (_rng.NextSingle() + 1)
                            * MathHelper.PiOver2
                            * (_rng.NextSingle() < 0.5f ? 1 : -1),
                    }
                );
        }
    }
}

internal class RectangleRenderer : Component, IRenderer
{
    public Vector2 Size { get; set; }
    public Color Color { get; set; } = Color.Orange;

    public int DrawOrder => 0;
    public bool IsVisible { get; set; } = true;

    public void Draw(SpriteBatch spriteBatch)
    {
        var drawHelper = Scene.Singletons.Get<DrawHelper>();
        var transform = Entity.Get<Transform>();
        drawHelper.DrawRectangle(
            spriteBatch,
            Color,
            transform.Position,
            Size * transform.Scale,
            normalizedOrigin: new Vector2(0.5f, 0.5f),
            transform.Rotation
        );
    }
}

internal class StaticGeometry : Component, ICollisionHandler
{
    void ICollisionHandler.OnCollisionEnter(in ContactInfo contact)
    {
        ICollisionHandler handler = this;
        handler.OnCollisionStay(contact);
    }

    void ICollisionHandler.OnCollisionStay(in ContactInfo contact)
    {
        if (contact.Other.Entity.Has<StaticGeometry>())
        {
            return;
        }
        var otherTransform = contact.Other.Entity.Get<Transform>();
        otherTransform.Position -= contact.Normal * contact.Overlap.Size;
    }
}

internal class Player : Component, IUpdatable, ICollisionHandler
{
    void IUpdatable.Update()
    {
        var inputManager = Scene.Singletons.Get<InputManager>();
        var velocity = Entity.Get<Velocity>();

        velocity.Linear.Y += 1500 * Scene.DeltaTime;
        if (inputManager.IsPressed(Keys.Space))
        {
            velocity.Linear.Y = -400;
        }

        velocity.Linear.X = 0;
        if (inputManager.IsDown(Keys.A))
        {
            velocity.Linear.X -= 200;
        }
        if (inputManager.IsDown(Keys.D))
        {
            velocity.Linear.X += 200;
        }
    }

    void ICollisionHandler.OnCollisionStay(in ContactInfo contact)
    {
        if (contact.Normal == new Vector2(0, -1))
        {
            var velocity = Entity.Get<Velocity>();
            velocity.Linear.Y = 0;
        }
    }
}
