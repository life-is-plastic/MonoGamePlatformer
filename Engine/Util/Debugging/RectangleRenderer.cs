using Engine.Core;
using Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Util.Debugging;

public partial class RectangleRenderer : Component
{
    public Vector2 Size { get; set; }
    public Vector2 NormalizedOrigin { get; set; }
    public Color Color { get; set; } = Color.Orange;
    public bool Filled = false;
}

public partial class RectangleRenderer : IRenderer
{
    public int DrawOrder => 0;
    public bool IsVisible { get; set; } = true;

    public void Draw(SpriteBatch spriteBatch)
    {
        var drawHelper = Scene.Singletons.Get<DrawHelper>();
        var transform = Entity.Get<Transform>();
        if (Filled)
        {
            drawHelper.DrawFilledRectangle(
                spriteBatch,
                Color,
                transform.Position,
                Size * transform.Scale,
                NormalizedOrigin,
                transform.Rotation
            );
        }
        else
        {
            drawHelper.DrawRectangle(
                spriteBatch,
                Color,
                transform.Position,
                Size * transform.Scale,
                NormalizedOrigin,
                transform.Rotation
            );
        }
    }
}
