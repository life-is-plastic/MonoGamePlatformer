using System.Collections.Generic;
using Engine.Core;
using Engine.Util.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Graphics;

public partial class RenderManager : Component
{
    public static int RenderWidth { get; } = 960;
    public static int RenderHeight { get; } = 540;
    public static Point RenderSize => new(RenderWidth, RenderHeight);

    private static readonly Comparer<IRenderer> s_drawOrderComparer = Comparer<IRenderer>.Create(
        (a, b) => (a.DrawOrder, a.Entity.Id).CompareTo((b.DrawOrder, b.Entity.Id))
    );

    private readonly IndexedSet<IRenderer> _renderers = new();
    private SpriteBatch _spriteBatch = null!;

    public override void Begin()
    {
        _spriteBatch = new SpriteBatch(Scene.Game.GraphicsDevice);
    }

    public override void End()
    {
        _spriteBatch.Dispose();
    }

    public void Draw()
    {
        Scene.Game.GraphicsDevice.Clear(Color.CornflowerBlue);
        _renderers.Sort(s_drawOrderComparer);

        var spriteBatchOptions = new IRenderer.DrawOptions();
        _spriteBatch.Begin(
            samplerState: spriteBatchOptions.SamplerState,
            effect: spriteBatchOptions.Effect
        );
        foreach (var renderer in _renderers)
        {
            if (!renderer.IsVisible)
            {
                continue;
            }
            if (!renderer.DrawOpts.Batch || renderer.DrawOpts != spriteBatchOptions)
            {
                _spriteBatch.End();
                _spriteBatch.Begin(
                    samplerState: renderer.DrawOpts.SamplerState,
                    effect: renderer.DrawOpts.Effect
                );
                spriteBatchOptions = renderer.DrawOpts;
            }
            renderer.Draw(_spriteBatch);
        }
        _spriteBatch.End();
    }
}

public partial class RenderManager : IEntitySyncer
{
    void IEntitySyncer.Sync(EntityChangelist entityChangelist)
    {
        foreach (var component in entityChangelist.Detached)
        {
            if (component is IRenderer renderer)
            {
                _renderers.RemoveOrDie(renderer);
            }
        }
        foreach (var component in entityChangelist.Attached)
        {
            if (component is IRenderer renderer)
            {
                _renderers.AddOrDie(renderer);
            }
        }
    }
}
