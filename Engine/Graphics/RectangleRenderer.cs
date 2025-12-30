using System.Diagnostics;
using Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Graphics;

/// <summary>
/// Simple rectangle renderer.
/// </summary>
public partial class RectangleRenderer : Component
{
    public Color Color { get; set; } = Color.DarkOrange;

    public Vector2 Size
    {
        get;
        set
        {
            Debug.Assert(value.X > 0 && value.Y > 0);
            field = value;
        }
    } = new(1, 1);

    public Vector2 Origin { get; set; } = default;

    public Vector2 NormalizedOrigin
    {
        get => Origin / Size;
        set => Origin = value * Size;
    }
}

public partial class RectangleRenderer : IRenderer
{
    public int DrawOrder { get; init; } = 0;
    public bool IsVisible { get; set; } = true;
    public IRenderer.Options RendererOptions { get; init; } = new();

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        var drawHelper = Scene.Singletons.Get<DrawHelper>();
        var transform = Entity.Get<Transform>();
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
