using System;
using Engine.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Engine;

public sealed partial class Scene
{
    private static readonly GameTime s_initialGameTime = new();
    private static readonly float s_maxDeltaTime = 0.05f;

    internal GameTime _gameTime = s_initialGameTime;
    internal readonly IndexedSet<Entity> _entities = new();
    internal readonly EntityChangelist _entityChangelist = new();
    private readonly EntityUpdater _entityUpdater = new();

    /// <summary>
    /// Human readable name for debugging.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The game instance owning this scene.
    /// </summary>
    public Game Game { get; }

    /// <summary>
    /// The content manager specific to this scene.
    /// </summary>
    public ContentManager Content { get; }

    /// <summary>
    /// Container entity for singleton components.
    /// </summary>
    public Entity Singletons { get; }

    public float DeltaTime { get; private set; } = 0;
    public float CurrentTime { get; private set; } = 0;
    public int FrameCount { get; private set; } = 0;
    public bool IsPaused => _entityUpdater.IsPaused;
    public bool ShouldPause { get; set; } = false;

    public Scene(ISceneDefinition sceneDefinition, Game game)
    {
        Name = sceneDefinition.Name();
        Game = game;
        Content = new ContentManager(game.Content.ServiceProvider)
        {
            RootDirectory = game.Content.RootDirectory,
        };
        Singletons = StageCreate(nameof(Singletons))
            .StageAttach(new InputManager())
            .StageAttach(new CollisionManager())
            .StageAttach(new AudioManager())
            .StageAttach(new MenuAudioManager())
            .StageAttach(new RenderManager())
            .StageAttach(new UIManager());

        StageCreate(nameof(Camera)).StageAttach(new Camera()).StageAttach(new Transform());
        Update();
        sceneDefinition.Initialize(this);
    }

    public override string ToString()
    {
        return Name;
    }

    /// <summary>
    /// Creates an entity and stages it to become part of the scene at the beginning of the next
    /// frame.
    /// </summary>
    public Entity StageCreate(string name)
    {
        return _entityChangelist.StageCreate(this, name);
    }

    public FindEntityEnumerable Find<T>()
        where T : IComponent
    {
        return new FindEntityEnumerable(_entities.AsSpan(), typeof(T));
    }

    public FindEntityEnumerable Find<T1, T2>()
        where T1 : IComponent
        where T2 : IComponent
    {
        return new FindEntityEnumerable(_entities.AsSpan(), typeof(T1), typeof(T2));
    }

    public FindEntityEnumerable Find<T1, T2, T3>()
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        return new FindEntityEnumerable(_entities.AsSpan(), typeof(T1), typeof(T2), typeof(T3));
    }

    internal void PreUpdate(GameTime gameTime)
    {
        FrameCount++;
        _gameTime = gameTime;
        DeltaTime = Math.Min(s_maxDeltaTime, (float)gameTime.ElapsedGameTime.TotalSeconds);
        CurrentTime += DeltaTime;
    }

    internal void Update()
    {
        var shouldPause = ShouldPause;
        _entityChangelist.Apply(_entities, _entityUpdater);
        _entityUpdater.ProcessPausing(shouldPause);
        _entityUpdater.Update();
    }

    internal void Draw()
    {
        Singletons.Get<RenderManager>().Draw();
    }
}

public sealed partial class Scene : IDisposable
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

    internal void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            _entityChangelist.Apply(_entities, _entityUpdater);
            foreach (var entity in _entities)
            {
                _entityChangelist.StageDestroy(entity);
            }
            _entityChangelist.Apply(_entities, _entityUpdater);

            Content.Dispose();
        }

        _isDisposed = true;
    }
}
