using System;
using Microsoft.Xna.Framework;

namespace Engine.Core;

public class Game : Microsoft.Xna.Framework.Game
{
    private Scene? _scene;

    public Scene? NextScene { get; set; }

    public Game(Scene initialScene)
    {
        _ = new GraphicsDeviceManager(this);

        Window.Title = "MonoGame Platformer";
        Window.AllowUserResizing = true;

        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        NextScene = initialScene;
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (NextScene is not null)
        {
            IDisposable? oldScene = _scene;
            (_scene, NextScene) = (NextScene, null);
            oldScene?.Dispose();
            GC.Collect();
            _scene.Initialize(this, gameTime);
        }
        _scene!.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        _scene!.Draw();
    }
}
