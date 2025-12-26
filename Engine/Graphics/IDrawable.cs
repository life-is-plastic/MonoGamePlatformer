using Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Graphics;

public interface IDrawable
{
    public void Draw(SpriteBatch spriteBatch, Transform transform, SpriteEffects effects)
    {
        Draw(spriteBatch, transform.Position, 0, transform.Scale, effects);
    }

    public void Draw(
        SpriteBatch spriteBatch,
        Vector2 position,
        float rotation,
        Vector2 scale,
        SpriteEffects effects
    );
}
