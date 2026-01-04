using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Debugging;

public class OriginRenderer : Component, IRenderer
{
    public Color Color { get; init; } = Color.DarkRed;

    int IRenderer.DrawOrder => IRenderer.DrawOrderDebug;

    void IRenderer.Draw(SpriteBatch spriteBatch)
    {
        var drawHelper = Scene.Singletons.Get<DrawHelper>();
        drawHelper.DrawLine(spriteBatch, Color, new Vector2(0, 0), new Vector2(8, 0));
        drawHelper.DrawLine(spriteBatch, Color, new Vector2(0, 0), new Vector2(0, 8));
    }
}
