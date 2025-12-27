using Engine.EC;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Graphics;

/// <summary>
/// Generic renderer for an arbitrary drawable type.
/// </summary>
public partial class DrawableRenderer<T> : Component
    where T : IDrawable
{
    public T Drawable { get; }
    public SpriteEffects SpriteEffects { get; set; } = SpriteEffects.None;

    public DrawableRenderer(in T drawable)
    {
        Drawable = drawable;
    }
}

public partial class DrawableRenderer<T> : IRenderer
{
    public int DrawOrder { get; init; } = 0;
    public bool IsVisible { get; set; } = true;
    public IRenderer.DrawOptions DrawOpts { get; init; } = new();

    void IRenderer.Draw(SpriteBatch spriteBatch)
    {
        Drawable.Draw(spriteBatch, Entity.Get<Transform>(), SpriteEffects);
    }
}
