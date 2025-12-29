using System;
using Engine.Audio;
using Engine.Core;
using Engine.Graphics;
using Engine.Input;
using Engine.Physics;
using Engine.Util;
using Engine.Util.Debugging;
using Library.Environment;
using Library.Player;
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
            .StageAttach(new CameraMouseZoom())
            // .StageAttach(new CameraMouseDrag())
            .StageAttach(new CameraFollowsPlayer())
            .StageAttach(new ColliderRenderer(Keys.P))
            .StageAttach(new DefaultSceneHelper())
            .StageAttach(new DrawHelper());

        World.MakeEntity(scene, 300, 180);
        MakeStaticGeometry(scene, new(200, 140), new(60, 20));
        MakeStaticGeometry(scene, new(100, 152), new(60, 20));

        scene
            .StageCreate(nameof(Player))
            .StageAttach(new PlayerMarker())
            .StageAttach(new PlayerController())
            .StageAttach(new Transform() { Position = new(50, 50) })
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

        scene
            .Singletons.Get<AudioManager>()
            .Play(scene.Content.Load<SoundEffect>("Audio/Theme"), loop: true);
    }

    private static void MakeStaticGeometry(Scene scene, Vector2 position, Vector2 size)
    {
        scene
            .StageCreate("Rect")
            .StageAttach(new Transform() { Position = position })
            .StageAttach(
                new RectangleRenderer()
                {
                    Size = size,
                    Color = Color.SaddleBrown,
                    Filled = true,
                }
            )
            .StageAttach(new Collider(size) { NormalizedOrigin = new(0.5f, 0.5f) })
            .StageAttach(new StaticGeometry());
    }
}

internal class DefaultSceneHelper : Component, IUpdatable
{
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
            var size = new Vector2(Random.Shared.Next(40, 80), Random.Shared.Next(20, 60));
            Scene
                .StageCreate("Rect")
                .StageAttach(
                    new Transform()
                    {
                        Position = new Vector2(
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
                    }
                )
                .StageAttach(
                    new RectangleRenderer()
                    {
                        Size = size,
                        Color = Color.SaddleBrown,
                        Filled = true,
                    }
                )
                .StageAttach(new Collider(size) { NormalizedOrigin = new(0.5f, 0.5f) })
                .StageAttach(new StaticGeometry());
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
