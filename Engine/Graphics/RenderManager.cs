using System;
using Engine.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine;

internal sealed class RenderManager : Component, IEntitySyncer
{
    private static readonly Comparison<IRenderer> s_drawOrderComparison = (a, b) =>
        (a.DrawOrder, a.Entity.Id).CompareTo((b.DrawOrder, b.Entity.Id));

    private readonly IndexedSet<IRenderer> _renderers = new();
    private RenderTarget2D _renderTarget = null!;
    private SpriteBatch _spriteBatch = null!;
    private EntityHandle _cameraHandle;

    protected override void Begin()
    {
        var cameraEntity = Scene.Find<Camera>().First();
        var camera = cameraEntity.Get<Camera>();
        _cameraHandle = new(cameraEntity);
        _renderTarget = new(Scene.Game.GraphicsDevice, camera.Width, camera.Height);
        _spriteBatch = new SpriteBatch(Scene.Game.GraphicsDevice);
    }

    protected override void End()
    {
        _renderTarget.Dispose();
        _spriteBatch.Dispose();
    }

    public void Draw()
    {
        var camera = _cameraHandle.Deref().Get<Camera>();
        DrawToRenderTarget(camera);
        DrawRenderTargetToScreen(camera);
    }

    private void DrawToRenderTarget(Camera camera)
    {
        _renderers.Sort(s_drawOrderComparison);

        var rendererOptions = new IRenderer.Options();

        var cameraRect = camera.AsWorldRectangleF();
        var cameraTransform = camera.Entity.Get<Transform>();
        var transformMatrix =
            Matrix.CreateScale(cameraTransform.Scale.X, cameraTransform.Scale.Y, 1)
            * Matrix.CreateTranslation(
                -cameraRect.X * cameraTransform.Scale.X,
                -cameraRect.Y * cameraTransform.Scale.Y,
                0
            );

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
        var renderTargetScreenSize = camera.ScreenScale * camera.Size.ToVector2();
        var renderTargetScreenPosition =
            (Scene.Game.ViewportSize.ToVector2() - renderTargetScreenSize) / 2;

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
