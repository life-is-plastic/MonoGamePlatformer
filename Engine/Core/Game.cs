using System;
using Engine.Graphics;
using Microsoft.Xna.Framework;

namespace Engine.Core;

public class Game : Microsoft.Xna.Framework.Game
{
    private readonly GraphicsDeviceManager _graphicsDeviceManager;
    private Scene _scene = null!;

    public Scene? NextScene { get; set; }

    public Game(Scene initialScene)
    {
        _graphicsDeviceManager = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = RenderManager.RenderWidth,
            PreferredBackBufferHeight = RenderManager.RenderHeight,
        };
        _graphicsDeviceManager.ApplyChanges();

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
            var nextScene = NextScene;
            NextScene = null;
            _scene?.Dispose();
            _scene = nextScene;
            GC.Collect();
            _scene.Initialize(this, gameTime);
        }
        _scene.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        _scene.Draw();
    }
}
