using System;
using Engine.Core;
using Engine.Util.Collections;
using Engine.Util.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Graphics;

internal sealed partial class RenderManager : Component
{
    private static readonly Comparison<IRenderer> s_drawOrderComparison = (a, b) =>
        (a.DrawOrder, a.Entity.Id).CompareTo((b.DrawOrder, b.Entity.Id));

    private readonly IndexedSet<IRenderer> _renderers = new();
    private RenderTarget2D _renderTarget = null!;
    private SpriteBatch _spriteBatch = null!;
    private EntityHandle _cameraEntity;

    protected override void Begin()
    {
        foreach (var (camera, _, entity) in Scene.Find<Camera, Transform>())
        {
            _cameraEntity = new(entity);
            _renderTarget = new(Scene.Game.GraphicsDevice, camera.Width, camera.Height);
            _spriteBatch = new SpriteBatch(Scene.Game.GraphicsDevice);
            return;
        }
        throw new InvalidOperationException($"unable to find ${typeof(Camera)}");
    }

    protected override void End()
    {
        _spriteBatch.Dispose();
    }

    public void Draw()
    {
        var camera = _cameraEntity.Deref().Get<Camera>();
        DrawToRenderTarget(camera);
        DrawRenderTargetToScreen(camera);
    }

    private void DrawToRenderTarget(Camera camera)
    {
        _renderers.Sort(s_drawOrderComparison);
        var rendererOptions = new IRenderer.Options();
        var cameraRect = camera.AsWorldRectangleF().Location;
        var transformMatrix = Matrix.CreateTranslation(-cameraRect.X, -cameraRect.Y, 0);

        Scene.Game.GraphicsDevice.SetRenderTarget(_renderTarget);
        Scene.Game.GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(rendererOptions, transformMatrix);

        foreach (var renderer in _renderers)
        {
            if (!renderer.IsVisible)
            {
                continue;
            }
            if (!renderer.RendererOptions.Batch || renderer.RendererOptions != rendererOptions)
            {
                _spriteBatch.End();
                _spriteBatch.Begin(renderer.RendererOptions, transformMatrix);
                rendererOptions = renderer.RendererOptions;
            }
            renderer.Draw(_spriteBatch);
        }

        _spriteBatch.End();
        Scene.Game.GraphicsDevice.SetRenderTarget(null);
    }

    private void DrawRenderTargetToScreen(Camera camera)
    {
        var screenSize = new Vector2(
            Scene.Game.GraphicsDevice.Viewport.Width,
            Scene.Game.GraphicsDevice.Viewport.Height
        );
        var scale = screenSize / camera.Size.ToVector2();
        var renderTargetScreenSize = Math.Min(scale.X, scale.Y) * camera.Size.ToVector2();
        var renderTargetScreenPosition = (screenSize - renderTargetScreenSize) / 2;

        _spriteBatch.Begin(new IRenderer.Options());
        _spriteBatch.Draw(
            _renderTarget,
            destinationRectangle: new Rectangle(
                (int)MathF.Round(renderTargetScreenPosition.X),
                (int)MathF.Round(renderTargetScreenPosition.Y),
                (int)MathF.Round(renderTargetScreenSize.X),
                (int)MathF.Round(renderTargetScreenSize.Y)
            ),
            Color.White
        );
        _spriteBatch.End();
    }
}

internal sealed partial class RenderManager : IEntitySyncer
{
    void IEntitySyncer.Sync(EntityChangelist entityChangelist)
    {
        foreach (var component in entityChangelist.Detached.Values)
        {
            if (component is IRenderer renderer)
            {
                _renderers.RemoveOrDie(renderer);
            }
        }
        foreach (var component in entityChangelist.Attached.Values)
        {
            if (component is IRenderer renderer)
            {
                _renderers.AddOrDie(renderer);
            }
        }
    }
}
