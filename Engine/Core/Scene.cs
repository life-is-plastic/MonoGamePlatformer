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

    public IndexedSetView<Entity> Entities => new(_entities);
    public bool IsPaused => _entityUpdater.IsPaused;
    public bool ShouldPause { get; set; } = false;
    public float DeltaTime => (float)GameTime.ElapsedGameTime.TotalSeconds;
    public float TotalTime => (float)GameTime.TotalGameTime.TotalSeconds;

    /// <summary>
    /// Human readable name for debugging.
    /// </summary>
    public string Name { get; }

    public Game Game { get; }

    /// <summary>
    /// Game time forwarded from <c>Game.Update()</c>. Non-MonoGame code must not mutate this
    /// object.
    /// </summary>
    public GameTime GameTime { get; private set; }

    public ContentManager Content { get; }

    public EntityChangelist EntityChangelist { get; }

    /// <summary>
    /// Container entity for singleton components.
    /// </summary>
    public Entity Singletons { get; }

    public Scene(ISceneDefinition sceneDefinition, Game game, GameTime initialGameTime)
    {
        Name = sceneDefinition.Name();
        Game = game;
        GameTime = initialGameTime;
        Content = new ContentManager(game.Content.ServiceProvider)
        {
            RootDirectory = game.Content.RootDirectory,
        };

        EntityChangelist = new(this);
        EntityChangelist
            .StageCreate(nameof(Camera))
            .StageAttach(new Camera())
            .StageAttach(new Transform());

        Singletons = EntityChangelist
            .StageCreate(nameof(Singletons))
            .StageAttach(new InputManager())
            .StageAttach(new CollisionManager())
            .StageAttach(new AudioManager())
            .StageAttach(new MenuAudioManager())
            .StageAttach(new RenderManager())
            .StageAttach(new UIManager());

        Update(initialGameTime);

        sceneDefinition.Initialize(this);
    }

    public override string ToString()
    {
        return Name;
    }

    /// <summary>
    /// Finds all entities with a component of type <c>T</c> (at the default component index) and
    /// yields <c>(T, entity)</c> pairs.
    /// </summary>
    public FindEntityEnumerable<T> Find<T>()
        where T : IComponent
    {
        return new FindEntityEnumerable<T>(_entities.AsSpan());
    }

    public FindEntityEnumerable<T1, T2> Find<T1, T2>()
        where T1 : IComponent
        where T2 : IComponent
    {
        return new FindEntityEnumerable<T1, T2>(_entities.AsSpan());
    }

    internal void Update(GameTime gameTime)
    {
        var shouldPause = ShouldPause;
        GameTime = gameTime;
        EntityChangelist.Apply(_entities, _entityUpdater);
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
