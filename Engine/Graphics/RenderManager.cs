using System;
using System.Collections.Generic;
using Engine.Core;
using Engine.Util.Collections;
using Engine.Util.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Graphics;

public partial class RenderManager : Component
{
    private static readonly Comparer<IRenderer> s_drawOrderComparer = Comparer<IRenderer>.Create(
        (a, b) => (a.DrawOrder, a.Entity.Id).CompareTo((b.DrawOrder, b.Entity.Id))
    );

    private readonly IndexedSet<IRenderer> _renderers = new();
    private RenderTarget2D _renderTarget = null!;
    private SpriteBatch _spriteBatch = null!;
    private EntityHandle _cameraEntity;

    public override void Begin()
    {
        foreach (var (camera, _, entity) in Scene.Find<Camera, Transform>())
        {
            _cameraEntity = new(entity);
            _renderTarget = new(Scene.Game.GraphicsDevice, camera.Width, camera.Height);
            _spriteBatch = new SpriteBatch(Scene.Game.GraphicsDevice);
            return;
        }
        throw new InvalidOperationException();
        // var (camera, _, entity) = Scene.Find<Camera, Transform>();
        // _cameraEntity = new(entity);
        // _renderTarget = new(Scene.Game.GraphicsDevice, camera.Width, camera.Height);
        // _spriteBatch = new SpriteBatch(Scene.Game.GraphicsDevice);
    }

    public override void End()
    {
        _spriteBatch.Dispose();
    }

    public void Draw()
    {
        var cameraEntity = _cameraEntity.Deref();
        DrawToRenderTarget(cameraEntity.Get<Transform>());
        DrawRenderTargetToScreen(cameraEntity.Get<Camera>());
    }

    private void DrawToRenderTarget(Transform cameraTransform)
    {
        _renderers.Sort(s_drawOrderComparer);
        var rendererDrawOptions = new IRenderer.DrawOptions();
        var transformMatrix =
            Matrix.CreateScale(cameraTransform.Scale.X, cameraTransform.Scale.Y, 1)
            * Matrix.CreateTranslation(-cameraTransform.Position.X, -cameraTransform.Position.Y, 0);

        Scene.Game.GraphicsDevice.SetRenderTarget(_renderTarget);
        Scene.Game.GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(rendererDrawOptions, transformMatrix);

        foreach (var renderer in _renderers)
        {
            if (!renderer.IsVisible)
            {
                continue;
            }
            if (!renderer.DrawOpts.Batch || renderer.DrawOpts != rendererDrawOptions)
            {
                _spriteBatch.End();
                _spriteBatch.Begin(renderer.DrawOpts, transformMatrix);
                rendererDrawOptions = renderer.DrawOpts;
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

        _spriteBatch.Begin(new IRenderer.DrawOptions());
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

public partial class RenderManager : IEntitySyncer
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
