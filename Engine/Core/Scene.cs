using System;
using Engine.Audio;
using Engine.Graphics;
using Engine.Input;
using Engine.Physics;
using Engine.UI;
using Engine.Util.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Engine.Core;

public sealed partial class Scene
{
    private readonly IndexedSet<Entity> _entities = new();
    private readonly EntityUpdater _entityUpdater = new();
    internal readonly EntityChangelist _entityChangelist = new();
    internal GameTime _gameTime;

    public IndexedSetView<Entity> Entities => new(_entities);
    public float DeltaTime => (float)_gameTime.ElapsedGameTime.TotalSeconds;
    public float TotalTime => (float)_gameTime.TotalGameTime.TotalSeconds;
    public bool IsPaused => _entityUpdater.IsPaused;
    public bool ShouldPause { get; set; } = false;

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

    public Scene(ISceneDefinition sceneDefinition, Game game, GameTime initialGameTime)
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

        _gameTime = initialGameTime;
        Update(initialGameTime);

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

    internal void Update(GameTime gameTime)
    {
        var shouldPause = ShouldPause;
        _gameTime = gameTime;
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
