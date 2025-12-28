using System;
using Engine.Audio;
using Engine.Core;
using Engine.Graphics;
using Engine.Input;
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
            .StageAttach(new DevSceneController());

        scene
            .EntityChangelist.StageCreate(nameof(RectRenderer))
            .StageAttach(new RectRenderer())
            .StageAttach(new RectRenderer() { ComponentIndex = 0 });

        foreach (var entity in scene.Entities)
        {
            if (entity.Has<Camera>())
            {
                entity.StageAttach(new CameraController());
                break;
            }
        }

        scene
            .Singletons.Get<AudioManager>()
            .Play(scene.Content.Load<SoundEffect>("Audio/Theme"), loop: true);
    }
}

internal class CameraController : Component, IUpdatable
{
    void IUpdatable.Update()
    {
        const float Speed = 240;
        var inputManager = Scene.Singletons.Get<InputManager>();
        var transform = Entity.Get<Transform>();
        if (inputManager.IsDown(Keys.A))
        {
            transform.Position.X -= Speed * Scene.DeltaTime;
        }
        if (inputManager.IsDown(Keys.D))
        {
            transform.Position.X += Speed * Scene.DeltaTime;
        }
        if (inputManager.IsDown(Keys.W))
        {
            transform.Position.Y -= Speed * Scene.DeltaTime;
        }
        if (inputManager.IsDown(Keys.S))
        {
            transform.Position.Y += Speed * Scene.DeltaTime;
        }
    }
}

internal class RectRenderer : Component, IRenderer
{
    private readonly Random _rng = new();
    private DrawUtil _drawUtil;
    private Vector2 _position;
    private float _rotation;
    private float _rotationSpeed;

    public int DrawOrder => 0;
    public bool IsVisible { get; set; } = true;

    protected override void Begin()
    {
        _drawUtil = new(Scene);
        _rotation = _rng.NextSingle() * MathF.PI * 2;
        _rotationSpeed =
            (_rng.NextSingle() + 1) * MathF.PI / 2 * (_rng.NextSingle() < 0.5f ? 1 : -1);
        foreach (var (camera, _) in Scene.Find<Camera>())
        {
            _position = new Vector2(_rng.NextInt64(camera.Width), _rng.NextInt64(camera.Height));
            return;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _rotation += Scene.IsPaused ? 0 : _rotationSpeed * Scene.DeltaTime;
        _drawUtil.DrawRectangle(
            spriteBatch,
            Color.DarkOrange,
            _position,
            size: new Vector2(60, 40),
            normalizedOrigin: new Vector2(0.5f, 0.5f),
            _rotation
        );
        _drawUtil.DrawLine(spriteBatch, Color.DarkOrchid, new Vector2(2, 2), new Vector2(40, 40));
    }
}

internal class DevSceneController : Component, IUpdatable
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
            Scene
                .EntityChangelist.StageCreate(nameof(RectRenderer))
                .StageAttach(new RectRenderer());
        }
        if (inputManager.IsPressed(Keys.D2))
        {
            foreach (var (_, entity) in Scene.Find<RectRenderer>())
            {
                Console.Out.WriteLine(entity);
            }
        }
    }
}
