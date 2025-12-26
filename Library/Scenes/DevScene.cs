using System;
using Engine.Audio;
using Engine.Core;
using Engine.Graphics;
using Engine.Input;
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
            .StageAttach(new RectRenderer(new Rectangle(100, 100, 60, 80)))
            .StageAttach(new RectRenderer(new Rectangle(200, 100, 50, 90)) { ComponentIndex = 0 });

        Singletons.Get<AudioManager>().Play(Content.Load<SoundEffect>("Audio/Theme"), loop: true);
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
    private Texture2D _pixel = null!;
    private Rectangle _dst;

    public int DrawOrder => 0;
    public bool IsVisible { get; set; } = true;

    public RectRenderer(Rectangle dst)
    {
        _dst = dst;
    }

    public override void Begin()
    {
        _pixel = new Texture2D(Scene.Game.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.DarkOrange]);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            _pixel,
            destinationRectangle: _dst,
            sourceRectangle: new Rectangle(0, 0, 1, 1),
            Color.White,
            rotation: Scene.TotalTime,
            origin: new(),
            SpriteEffects.None,
            layerDepth: 0
        );
    }
}
