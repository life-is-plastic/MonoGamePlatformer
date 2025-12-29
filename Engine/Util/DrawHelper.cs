using System;
using Engine.Core;
using Engine.Util.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Util;

/// <summary>
/// Helper for drawing various shapes.
/// </summary>
public class DrawHelper : Component
{
    private Texture2D _pixel = null!;

    protected override void Begin()
    {
        _pixel = new Texture2D(Scene.Game.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);
    }

    protected override void End()
    {
        _pixel.Dispose();
    }

    public void DrawLine(
        SpriteBatch spriteBatch,
        Color color,
        Vector2 position1,
        Vector2 position2,
        float thickness = 1
    )
    {
        var diff = position2 - position1;
        var len = diff.Length();
        var rotation = diff.Rotation();
        spriteBatch.Draw(
            _pixel,
            position1,
            sourceRectangle: null,
            color,
            rotation,
            origin: default,
            scale: new Vector2(len, thickness),
            SpriteEffects.None,
            layerDepth: 0
        );
    }

    public void DrawRectangle(
        SpriteBatch spriteBatch,
        Color color,
        Vector2 position,
        Vector2 size,
        Vector2 normalizedOrigin = default,
        float rotation = 0,
        float thickness = 1
    )
    {
        var rect = new RectangleF(position - size * normalizedOrigin, size);
        var tl = new Vector2(rect.Left, rect.Top);
        var tr = new Vector2(rect.Right, rect.Top);
        var bl = new Vector2(rect.Left, rect.Bottom);
        var br = new Vector2(rect.Right, rect.Bottom);
        tl.RotateAround(position, rotation);
        tr.RotateAround(position, rotation);
        bl.RotateAround(position, rotation);
        br.RotateAround(position, rotation);
        DrawLine(spriteBatch, color, tl, tr, thickness);
        DrawLine(spriteBatch, color, tr, br, thickness);
        DrawLine(spriteBatch, color, tl, bl, thickness);
        DrawLine(spriteBatch, color, bl, br, thickness);
    }

    public void DrawFilledRectangle(
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
            sourceRectangle: null,
            color,
            rotation,
            normalizedOrigin,
            SpriteEffects.None,
            layerDepth: 0
        );
    }
}
