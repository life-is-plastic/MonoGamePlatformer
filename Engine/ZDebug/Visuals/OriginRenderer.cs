using Engine.Core;
using Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.ZDebug.Visuals;

public partial class OriginRenderer : Component { }

public partial class OriginRenderer : IRenderer
{
    public int DrawOrder => 100;
    public bool IsVisible { get; set; } = true;

    public void Draw(SpriteBatch spriteBatch)
    {
        var color = Color.DarkRed;
        var drawHelper = Scene.Singletons.Get<DrawHelper>();
        drawHelper.DrawLine(spriteBatch, color, new Vector2(0, 0), new Vector2(8, 0));
        drawHelper.DrawLine(spriteBatch, color, new Vector2(0, 0), new Vector2(0, 8));
    }
}
