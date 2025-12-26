using Engine.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Graphics;

public readonly record struct TextureRegion
{
    public Texture2D Texture { get; }
    public Rectangle Region { get; }
    public RectangleF NormalizedRegion =>
        new(
            (float)Region.X / Texture.Width,
            (float)Region.Y / Texture.Height,
            (float)Region.Width / Texture.Width,
            (float)Region.Height / Texture.Height
        );

    /// <summary>
    /// Creates a texture region covering the entire texture.
    /// </summary>
    public TextureRegion(Texture2D texture)
        : this(texture, new Rectangle(0, 0, texture.Width, texture.Height)) { }

    /// <summary>
    /// Creates a texture region covering the given region.
    /// </summary>
    public TextureRegion(Texture2D texture, Rectangle region)
    {
        Texture = texture;
        Region = region;
    }

    /// <summary>
    /// Creates a texture region from another texture region. <c>region</c> is relative to the input
    /// texture region rather than the underlying source texture.
    /// </summary>
    public TextureRegion(in TextureRegion textureRegion, Rectangle region)
        : this(
            textureRegion.Texture,
            new Rectangle(textureRegion.Region.Location + region.Location, region.Size)
        ) { }

    public struct DrawOptions
    {
        public float Rotation = 0;
        public Vector2 Scale = Vector2.One;
        public Vector2 Origin = Vector2.Zero;
        public Color Tint = Color.White;
        public SpriteEffects Effects = SpriteEffects.None;
        public float LayerDepth = 0;

        public DrawOptions() { }
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        Draw(spriteBatch, position, new DrawOptions());
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, in DrawOptions options)
    {
        spriteBatch.Draw(
            Texture,
            position,
            Region,
            options.Tint,
            options.Rotation,
            options.Origin,
            options.Scale,
            options.Effects,
            options.LayerDepth
        );
    }
}
