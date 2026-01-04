using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Debugging;

public class GridRenderer : Component, IRenderer
{
    public Color Color { get; init; } = Color.DarkRed;
    public float Spacing { get; init; } = 16;

    int IRenderer.DrawOrder => IRenderer.DrawOrderDebug;

    void IRenderer.Draw(SpriteBatch spriteBatch)
    {
        var drawHelper = Scene.Singletons.Get<DrawHelper>();
        for (var i = 0; i < 100; i++)
        {
            drawHelper.DrawLine(
                spriteBatch,
                Color,
                new Vector2(i * Spacing, -1000),
                new Vector2(i * Spacing, 1000)
            );
            drawHelper.DrawLine(
                spriteBatch,
                Color,
                new Vector2(-1000, i * Spacing),
                new Vector2(1000, i * Spacing)
            );
        }
    }
}
