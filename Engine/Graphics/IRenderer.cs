using Microsoft.Xna.Framework.Graphics;

namespace Engine;

/// <summary>
/// Interface for renderer components.
/// </summary>
public interface IRenderer : IComponent
{
    // Preset draw orders.
    public static int DrawOrderDefault => 0;
    public static int DrawOrderUI => 1000;
    public static int DrawOrderDebug => 1100;

    /// <summary>
    /// Determines when this component is drawn. A higher value means this renderer is drawn on top
    /// of those with lower values. This property must return a fixed value for the lifetime of the
    /// component.
    /// </summary>
    public int DrawOrder => DrawOrderDefault;

    /// <summary>
    /// If false, then <c>Draw()</c> will not be called.
    /// </summary>
    public bool IsVisible => true;

    /// <summary>
    /// Configures sprite batch <c>Begin()</c>.
    /// </summary>
    public Options RendererOptions => new();

    /// <summary>
    /// Draws the component.
    /// </summary>
    /// <param name="spriteBatch">
    /// The scene-global sprite batch owned by <c>RenderManager</c>.
    /// </param>
    public void Draw(SpriteBatch spriteBatch);

    public record struct Options
    {
        public bool Batch = true;
        public SamplerState? SamplerState = SamplerState.PointClamp;
        public Effect? Effect = null;

        public Options() { }

        public readonly void Begin(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState, effect: Effect);
        }
    }
}
