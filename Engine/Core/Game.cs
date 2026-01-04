using System;
using Microsoft.Xna.Framework;

namespace Engine;

public sealed class Game : Microsoft.Xna.Framework.Game
{
    private Scene? _scene;

    public ISceneDefinition? NextSceneDefinition { get; set; }
    public Point ViewportSize => new(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

    public Game(ISceneDefinition initialSceneDefinition)
    {
        var gdm = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = 1920,
            PreferredBackBufferHeight = 1080,
        };
        gdm.ApplyChanges();

        Window.Title = "MonoGame Platformer";
        Window.AllowUserResizing = true;

        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        NextSceneDefinition = initialSceneDefinition;
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (NextSceneDefinition is not null)
        {
            IDisposable? oldScene = _scene;
            oldScene?.Dispose();
            _scene = null;
            GC.Collect();
            _scene = new Scene(NextSceneDefinition, this);
            NextSceneDefinition = null;
        }
        _scene!.PreUpdate(gameTime);
        _scene!.Update();
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        _scene!.Draw();
    }
}
