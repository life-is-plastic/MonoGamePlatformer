using Engine.Core;
using Engine.Graphics;
using Engine.Input;
using Engine.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine.Util.Debugging;

/// <summary>
/// For debug collider visualization.
/// </summary>
public partial class ColliderRenderer : Component
{
    private readonly Keys _toggleKey;

    public ColliderRenderer(Keys toggleKey = Keys.None)
    {
        _toggleKey = toggleKey;
    }
}

public partial class ColliderRenderer : IUpdatable
{
    bool IUpdatable.Pause()
    {
        return false;
    }

    void IUpdatable.Update()
    {
        var inputManager = Scene.Singletons.Get<InputManager>();
        if (inputManager.IsPressed(_toggleKey))
        {
            IsVisible = !IsVisible;
        }
    }
}

public partial class ColliderRenderer : IRenderer
{
    public int DrawOrder => 100;
    public bool IsVisible { get; set; } = true;

    void IRenderer.Draw(SpriteBatch spriteBatch)
    {
        var drawHelper = Scene.Singletons.Get<DrawHelper>();
        var collisionManager = Scene.Singletons.Get<CollisionManager>();
        foreach (var colliders in collisionManager._colliders.Values)
        {
            foreach (var collider in colliders)
            {
                var rect = collider.AsWorldRectangleF();
                drawHelper.DrawRectangle(spriteBatch, Color.Orange, rect.Location, rect.Size);
            }
        }
    }
}
