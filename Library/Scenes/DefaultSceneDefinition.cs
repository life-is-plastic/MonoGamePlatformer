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
            .StageAttach(new CameraMouseZoom())
            .StageAttach(new ColliderRenderer(Keys.P))
            .StageAttach(new DefaultSceneHelper())
            .StageAttach(new DrawHelper());

        Player.MakeEntity(scene);
        World.MakeEntity(scene, 300, 180);

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
            .StageAttach(
                new RectangleRenderer()
                {
                    Size = size,
                    Color = color,
                    Filled = true,
                }
            )
            .StageAttach(new Collider(size) { NormalizedOrigin = new(0.5f, 0.5f) });
    }

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
                        MathHelper.Lerp(
                            cameraRect.Left,
                            cameraRect.Right,
                            Random.Shared.NextSingle()
                        ),
                        MathHelper.Lerp(
                            cameraRect.Top,
                            cameraRect.Bottom,
                            Random.Shared.NextSingle()
                        )
                    ),
                    size: new Vector2(Random.Shared.Next(40, 80), Random.Shared.Next(20, 60)),
                    color: Color.SaddleBrown
                )
                .StageAttach(new StaticGeometry())
                .StageAttach(
                    new Velocity
                    {
                        Angular =
                            (Random.Shared.NextSingle() + 1)
                            * MathHelper.PiOver2
                            * (Random.Shared.NextSingle() < 0.5f ? 1 : -1),
                    }
                );
        }
    }
}

internal class RectangleRenderer : Component, IRenderer
{
    public Vector2 Size { get; set; }
    public Color Color { get; set; } = Color.Orange;
    public bool Filled = false;

    public int DrawOrder => 0;
    public bool IsVisible { get; set; } = true;

    public void Draw(SpriteBatch spriteBatch)
    {
        var drawHelper = Scene.Singletons.Get<DrawHelper>();
        var transform = Entity.Get<Transform>();
        if (Filled)
        {
            drawHelper.DrawFilledRectangle(
                spriteBatch,
                Color,
                transform.Position,
                Size * transform.Scale,
                normalizedOrigin: new Vector2(0.5f, 0.5f),
                transform.Rotation
            );
        }
        else
        {
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
        if (contact.Other.Entity.MaybeGet<Velocity>() is { } velocity)
        {
            // Zero out the velocity component parallel to the normal.
            var v = contact.Normal;
            v.Rotate(MathHelper.PiOver2);
            v *= v;
            velocity.Linear *= v;
        }
    }
}

internal class Player : Component, IUpdatable
{
    public static Entity MakeEntity(Scene scene)
    {
        return scene
            .StageCreate(nameof(Player))
            .StageAttach(new Player())
            .StageAttach(new Transform())
            .StageAttach(new Velocity())
            .StageAttach(new Collider(20, 20) { NormalizedOrigin = new(0.5f, 0.5f) })
            .StageAttach(
                new RectangleRenderer()
                {
                    Size = new(20, 20),
                    Color = Color.DarkGray,
                    Filled = true,
                }
            );
    }

    void IUpdatable.Update()
    {
        var inputManager = Scene.Singletons.Get<InputManager>();
        var velocity = Entity.Get<Velocity>();

        velocity.Linear.Y += 500 * Scene.DeltaTime;
        if (inputManager.IsPressed(Keys.Space))
        {
            velocity.Linear.Y = -200;
        }

        var dv = 300;
        if (!(inputManager.IsDown(Keys.A) ^ inputManager.IsDown(Keys.D)))
        {
            velocity.Linear.X = 0;
        }
        else if (inputManager.IsDown(Keys.A))
        {
            velocity.Linear.X = -dv;
        }
        else if (inputManager.IsDown(Keys.D))
        {
            velocity.Linear.X = dv;
        }
    }
}

internal class World : Component
{
    public static Entity MakeEntity(Scene scene, float width, float height)
    {
        var margin = 64;

        return scene
            .StageCreate(nameof(World))
            .StageAttach(new World())
            .StageAttach(new Transform())
            .StageAttach(new StaticGeometry())
            .StageAttach(
                new Collider(width + 2 * margin, margin)
                {
                    Origin = new(margin, margin),
                    ComponentIndex = -1,
                }
            )
            .StageAttach(
                new Collider(width + 2 * margin, margin)
                {
                    Origin = new(margin, -height),
                    ComponentIndex = -2,
                }
            )
            .StageAttach(
                new Collider(margin, height) { Origin = new(margin, 0), ComponentIndex = -3 }
            )
            .StageAttach(
                new Collider(margin, height) { Origin = new(-width, 0), ComponentIndex = -4 }
            );
    }
}
