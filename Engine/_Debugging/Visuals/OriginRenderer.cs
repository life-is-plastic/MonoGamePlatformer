using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Debugging;

public partial class OriginRenderer : Component { }

public partial class OriginRenderer : IRenderer
{
    int IRenderer.DrawOrder => IRenderer.DrawOrderDebug;

    void IRenderer.Draw(SpriteBatch spriteBatch)
    {
        var color = Color.DarkRed;
        var drawHelper = Scene.Singletons.Get<DrawHelper>();
        drawHelper.DrawLine(spriteBatch, color, new Vector2(0, 0), new Vector2(8, 0));
        drawHelper.DrawLine(spriteBatch, color, new Vector2(0, 0), new Vector2(0, 8));
    }
}
