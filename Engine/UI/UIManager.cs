using Gum.Forms.Controls;
using Gum.Wireframe;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;

namespace Engine;

/// <summary>
/// Component wrapper around a GumService instance.
/// </summary>
public partial class UIManager : Component, IUpdatable
{
    private static bool s_gumServiceInitialized = false;

    public GumService GumService { get; } = GumService.Default;

    public void AddToRoot(GraphicalUiElement element)
    {
        GumService.Root.Children.Add(element);
    }

    public void AddToRoot(FrameworkElement element)
    {
        GumService.Root.Children.Add(element.Visual);
    }

    protected override void Begin()
    {
        if (s_gumServiceInitialized)
        {
            return;
        }
        GumService.Initialize(Scene.Game, Gum.Forms.DefaultVisualsVersion.V2);
        GumService.ContentLoader?.XnaContentManager = Scene.Game.Content;
        GumService.Renderer.Camera.Zoom = 3;
        GumService.CanvasWidth =
            Scene.Game.GraphicsDevice.PresentationParameters.BackBufferWidth
            / GumService.Renderer.Camera.Zoom;
        GumService.CanvasHeight =
            Scene.Game.GraphicsDevice.PresentationParameters.BackBufferHeight
            / GumService.Renderer.Camera.Zoom;
        FrameworkElement.KeyboardsForUiControl.Add(GumService.Keyboard);
        s_gumServiceInitialized = true;
    }

    protected override void End()
    {
        GumService.Root.Children.Clear();
    }

    int IUpdatable.UpdateOrder => IUpdatable.UpdateOrderFrameBegin;

    bool IUpdatable.Pause()
    {
        return false;
    }

    void IUpdatable.Update()
    {
        if (IsVisible)
        {
            GumService.Update(Scene._gameTime);
        }
    }
}

public partial class UIManager : IRenderer
{
    int IRenderer.DrawOrder => IRenderer.DrawOrderUI;
    public bool IsVisible { get; set; } = true;
    IRenderer.Options IRenderer.RendererOptions => new() { Batch = false };

    void IRenderer.Draw(SpriteBatch spriteBatch)
    {
        GumService.Draw();
    }
}
