using Microsoft.Xna.Framework.Graphics;

namespace Engine;

/// <summary>
/// Generic renderer for an arbitrary drawable type.
/// </summary>
public partial class DrawableRenderer<T> : Component
    where T : IDrawable
{
    public required T Drawable { get; init; }
    public SpriteEffects SpriteEffects { get; set; } = SpriteEffects.None;
}

public partial class DrawableRenderer<T> : IRenderer
{
    public int DrawOrder { get; init; } = 0;
    public bool IsVisible { get; set; } = true;
    public IRenderer.Options RendererOptions { get; init; } = new();

    void IRenderer.Draw(SpriteBatch spriteBatch)
    {
        Drawable.Draw(spriteBatch, Entity.Get<Transform>(), SpriteEffects);
    }
}
