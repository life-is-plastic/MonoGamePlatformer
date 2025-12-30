using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Graphics;

public partial class Sprite
{
    public TextureRegion TextureRegion { get; }

    /// <summary>
    /// Origin in pixel units.
    /// <para>Note that origin refers to the top-left corner of the specified pixel, not the center
    /// of the pixel.</para>
    /// </summary>
    public Vector2 Origin { get; set; } = new();

    /// <summary>
    /// 0 anchors to the left/top, 0.5 anchors to the center, and 1 anchors to the right/bottom.
    /// Values outside [0, 1] are also valid.
    /// </summary>
    public Vector2 NormalizedOrigin
    {
        get => Origin / TextureRegion.Region.Size.ToVector2();
        set => Origin = value * TextureRegion.Region.Size.ToVector2();
    }

    public Sprite(in TextureRegion textureRegion)
    {
        TextureRegion = textureRegion;
    }
}

public partial class Sprite : IDrawable
{
    public void Draw(
        SpriteBatch spriteBatch,
        Vector2 position,
        float rotation,
        Vector2 scale,
        SpriteEffects effects
    )
    {
        TextureRegion.Draw(
            spriteBatch,
            position,
            new TextureRegion.DrawOptions
            {
                Rotation = rotation,
                Scale = scale,
                Origin = Origin,
                Effects = effects,
            }
        );
    }
}
