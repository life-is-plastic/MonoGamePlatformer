using Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Util.Extensions;

public static class SpriteBatchExtensions
{
    extension(SpriteBatch spriteBatch)
    {
        public void Begin(in IRenderer.DrawOptions drawOptions, in Matrix? transformMatrix = null)
        {
            spriteBatch.Begin(
                samplerState: drawOptions.SamplerState,
                effect: drawOptions.Effect,
                transformMatrix: transformMatrix
            );
        }
    }
}
