using Engine.Core;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Graphics;

/// <summary>
/// Interface for renderer components.
/// </summary>
public interface IRenderer : IComponent
{
    public record struct DrawOptions
    {
        public bool Batch = true;
        public SamplerState? SamplerState = SamplerState.PointClamp;
        public Effect? Effect = null;

        public DrawOptions() { }
    }

    /// <summary>
    /// Determines when this component is drawn. A higher value means this renderer is drawn on top
    /// of those with lower values. This property must return a fixed value for the lifetime of the
    /// component.
    /// </summary>
    public int DrawOrder { get; }

    /// <summary>
    /// If false, then <c>Draw()</c> will not be called.
    /// </summary>
    public bool IsVisible { get; set; }

    /// <summary>
    /// Configures sprite batch <c>Begin()</c>.
    /// </summary>
    public DrawOptions DrawOpts => new();

    /// <summary>
    /// Draws the component.
    /// </summary>
    /// <param name="spriteBatch">
    /// The scene-global sprite batch owned by <c>RenderManager</c>.
    /// </param>
    public void Draw(SpriteBatch spriteBatch);
}
