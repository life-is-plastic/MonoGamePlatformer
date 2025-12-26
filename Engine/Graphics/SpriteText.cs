using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Graphics;

public partial class SpriteText
{
    public SpriteFont SpriteFont { get; }

    public string Message
    {
        get;
        set
        {
            field = value;
            Size = SpriteFont.MeasureString(value);
        }
    } = "";

    /// <summary>
    /// Pixel dimensions of final sprite.
    /// </summary>
    public Vector2 Size { get; private set; }

    public Color Color { get; set; } = Color.White;

    /// <summary>
    /// Origin in pixel units.
    /// </summary>
    public Vector2 Origin => Size * NormalizedOrigin + OriginOffset;

    /// <summary>
    /// 0 anchors to the left/top, 0.5 anchors to the center, and 1 anchors to the right/bottom.
    /// Values outside [0, 1] are also valid.
    /// </summary>
    public Vector2 NormalizedOrigin { get; set; } = Vector2.Zero;

    /// <summary>
    /// Pixel offset applied on top of <c>NormalizedOrigin</c> to derive the final origin.
    /// </summary>
    public Vector2 OriginOffset { get; set; } = Vector2.Zero;

    public SpriteText(SpriteFont spriteFont)
    {
        SpriteFont = spriteFont;
    }
}

public partial class SpriteText : IDrawable
{
    public void Draw(
        SpriteBatch spriteBatch,
        Vector2 position,
        float rotation,
        Vector2 scale,
        SpriteEffects effects
    )
    {
        spriteBatch.DrawString(
            SpriteFont,
            Message,
            position,
            Color,
            rotation,
            Origin,
            scale,
            effects,
            layerDepth: 0
        );
    }
}
