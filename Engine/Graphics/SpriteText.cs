using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine;

public partial class SpriteText
{
    private bool _isSizeDirty = true;

    public required SpriteFont SpriteFont { get; init; }

    public string Message
    {
        get;
        set
        {
            if (field != value)
            {
                _isSizeDirty = true;
            }
            field = value;
        }
    } = "";

    /// <summary>
    /// Pixel dimensions of final sprite.
    /// </summary>
    public Vector2 Size
    {
        get
        {
            if (_isSizeDirty)
            {
                field = SpriteFont.MeasureString(Message);
                _isSizeDirty = false;
            }
            return field;
        }
    }

    public Color Color { get; set; } = Color.White;

    /// <summary>
    /// Origin in pixel units.
    /// </summary>
    public Vector2 Origin => Size * NormalizedOrigin + OriginOffset;

    /// <summary>
    /// 0 anchors to the left/top, 0.5 anchors to the center, and 1 anchors to the right/bottom.
    /// Values outside [0, 1] are also valid.
    /// </summary>
    public Vector2 NormalizedOrigin { get; set; } = new();

    /// <summary>
    /// Pixel offset applied on top of <c>NormalizedOrigin</c> to derive the final origin.
    /// </summary>
    public Vector2 OriginOffset { get; set; } = new();
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
