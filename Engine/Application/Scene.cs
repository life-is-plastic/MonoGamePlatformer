using System;
using Engine.Audio;
using Engine.EC;
using Engine.Graphics;
using Engine.Input;
using Engine.Physics;
using Engine.UI;
using Engine.Util.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Engine.Application;

public abstract partial class Scene
{
    /// <summary>
    /// Human readable name for debugging.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Called at the end of <c>Initialize(Game, GameTime)</c>. Initial scene entities should be
    /// created here.
    /// </summary>
    public abstract void Initialize();
}

public abstract partial class Scene
{
    private readonly IndexedSet<Entity> _entities = new();
    private readonly EntityUpdater _entityUpdater = new();

    public IndexedSetView<Entity> Entities => new(_entities);
    public bool IsPaused => _entityUpdater.IsPaused;
    public bool ShouldPause { get; set; } = false;

    public Game Game { get; private set; } = null!;
    public ContentManager Content { get; private set; } = null!;
    public EntityChangelist EntityChangelist { get; private set; } = null!;

    /// <summary>
    /// Container entity for singleton components.
    /// </summary>
    public Entity Singletons { get; private set; } = null!;

    /// <summary>
    /// Game time forwarded from <c>Game</c>. Non-MonoGame code must not mutate this object.
    /// </summary>
    public GameTime GameTime { get; private set; } = null!;

    /// <summary>
    /// Seconds since the previous frame.
    /// </summary>
    public float DeltaTime => (float)GameTime.ElapsedGameTime.TotalSeconds;

    public float TotalTime => (float)GameTime.TotalGameTime.TotalSeconds;

    public override string ToString()
    {
        return Name;
    }

    /// <summary>
    /// Initializes scene fields and creates required singleton components.
    /// </summary>
    public void Initialize(Game game, GameTime gameTime)
    {
        Game = game;
        Content = new ContentManager(game.Content.ServiceProvider)
        {
            RootDirectory = game.Content.RootDirectory,
        };
        EntityChangelist = new(this);

        Singletons = EntityChangelist
            .StageCreate(nameof(Singletons))
            .StageAttach(new InputManager())
            .StageAttach(new CollisionManager())
            .StageAttach(new AudioManager())
            .StageAttach(new MenuAudioManager())
            .StageAttach(new RenderManager())
            .StageAttach(new UIManager());
        EntityChangelist
            .StageCreate(nameof(Camera))
            .StageAttach(new Camera())
            .StageAttach(new Transform());
        Update(gameTime);

        Initialize();
    }

    public void Update(GameTime gameTime)
    {
        var shouldPause = ShouldPause;
        GameTime = gameTime;
        EntityChangelist.Apply(_entities, _entityUpdater);
        _entityUpdater.ProcessPausing(shouldPause);
        _entityUpdater.Update();
    }

    public void Draw()
    {
        Singletons.Get<RenderManager>().Draw();
    }
}

public abstract partial class Scene : IDisposable
{
    private bool _isDisposed = false;

    ~Scene()
    {
        Dispose(false);
    }

    void IDisposable.Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            EntityChangelist.Apply(_entities, _entityUpdater);
            foreach (var entity in _entities)
            {
                EntityChangelist.StageDestroy(entity);
            }
            EntityChangelist.Apply(_entities, _entityUpdater);

            Content.Dispose();
        }

        _isDisposed = true;
    }
}
