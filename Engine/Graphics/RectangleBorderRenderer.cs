using Engine.Core;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Graphics;

/// <summary>
/// Simple rectangle border renderer.
/// </summary>
public class RectangleBorderRenderer : RectangleRenderer
{
    public float BorderWidth { get; set; } = 1;

    public override void Draw(SpriteBatch spriteBatch)
    {
        var drawHelper = Scene.Singletons.Get<DrawHelper>();
        var transform = Entity.Get<Transform>();
        drawHelper.DrawRectangleBorder(
            spriteBatch,
            Color,
            transform.Position,
            Size * transform.Scale,
            NormalizedOrigin,
            transform.Rotation,
            BorderWidth
        );
    }
}
