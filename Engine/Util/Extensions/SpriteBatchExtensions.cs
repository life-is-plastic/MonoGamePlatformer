using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Util;

public static class SpriteBatchExtensions
{
    extension(SpriteBatch spriteBatch)
    {
        public void Begin(in IRenderer.Options rendererOptions, in Matrix? transformMatrix = null)
        {
            spriteBatch.Begin(
                samplerState: rendererOptions.SamplerState,
                effect: rendererOptions.Effect,
                transformMatrix: transformMatrix
            );
        }
    }
}
