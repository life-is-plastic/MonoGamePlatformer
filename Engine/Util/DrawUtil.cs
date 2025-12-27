using System;
using Engine.App;
using Engine.Util.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Util;

/// <summary>
/// Helper for drawing various shapes.
/// </summary>
public readonly struct DrawUtil
{
    private readonly Texture2D _pixel;

    public DrawUtil(Scene scene)
    {
        _pixel = new Texture2D(scene.Game.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);
    }

    public void DrawLine(SpriteBatch spriteBatch, Color color, Vector2 position1, Vector2 position2)
    {
        var diff = position2 - position1;
        var len = diff.Length();
        var rotation = diff.Rotation();
        DrawRectangle(spriteBatch, color, position1, new(len, 1), rotation: rotation);
    }

    public void DrawRectangle(
        SpriteBatch spriteBatch,
        Color color,
        Vector2 position,
        Vector2 size,
        Vector2 normalizedOrigin = default,
        float rotation = 0
    )
    {
        spriteBatch.Draw(
            _pixel,
            // Destination rectangle's x and y is where the origin gets placed.
            destinationRectangle: new Rectangle(
                (int)MathF.Round(position.X),
                (int)MathF.Round(position.Y),
                (int)MathF.Round(size.X),
                (int)MathF.Round(size.Y)
            ),
            sourceRectangle: new Rectangle(0, 0, 1, 1),
            color,
            rotation,
            origin: normalizedOrigin,
            SpriteEffects.None,
            layerDepth: 0
        );
    }
}
