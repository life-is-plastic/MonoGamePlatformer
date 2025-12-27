using Engine.App;
using Engine.Audio;
using Engine.EC;
using Engine.Graphics;
using Engine.Input;
using Engine.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Library.Scenes;

public class DevScene : Scene
{
    public override string Name => nameof(DevScene);

    public override void Initialize()
    {
        Singletons.StageAttach(new DevSceneController());

        EntityChangelist
            .StageCreate(nameof(RectRenderer))
            .StageAttach(new RectRenderer(new Vector2(200, 100)))
            .StageAttach(new RectRenderer(new Vector2(100, 40)) { ComponentIndex = 0 });

        foreach (var entity in Entities)
        {
            if (entity.Has<Camera>())
            {
                entity.StageAttach(new CameraController());
                break;
            }
        }

        Singletons.Get<AudioManager>().Play(Content.Load<SoundEffect>("Audio/Theme"), loop: true);
    }
}

internal class CameraController : Component, IUpdatable
{
    void IUpdatable.Update()
    {
        const float Speed = 120;
        var inputManager = GetSingleton<InputManager>();
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

internal class DevSceneController : Component, IUpdatable
{
    bool IUpdatable.Pause()
    {
        return false;
    }

    void IUpdatable.Update()
    {
        var inputManager = GetSingleton<InputManager>();
        if (inputManager.IsPressed(Keys.Escape))
        {
            Scene.ShouldPause = !Scene.IsPaused;
        }
    }
}

internal class RectRenderer : Component, IRenderer
{
    private DrawUtil _drawUtil;
    private Vector2 _position;

    public int DrawOrder => 0;
    public bool IsVisible { get; set; } = true;

    public RectRenderer(Vector2 position)
    {
        _position = position;
    }

    public override void Begin()
    {
        _drawUtil = new(Scene);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _drawUtil.DrawRectangle(
            spriteBatch,
            Color.DarkOrange,
            _position,
            size: new(90, 60),
            normalizedOrigin: new(0.5f, 0.5f),
            rotation: Scene.TotalTime
        );
        _drawUtil.DrawLine(spriteBatch, Color.DarkOrchid, new(2, 2), new(40, 40));
    }
}
