using Engine.Core;
using Engine.Graphics;
using Engine.Input;
using Engine.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine.ZDebug.Visuals;

public partial class ColliderRenderer : Component
{
    private readonly Button _toggleButton;

    public ColliderRenderer(Button? toggleButton = null)
    {
        _toggleButton = toggleButton ?? Keys.P;
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
        if (inputManager.IsPressed(_toggleButton))
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
        foreach (var colliders in collisionManager._layerToColliders)
        {
            foreach (var collider in colliders)
            {
                var rect = collider.AsWorldRectangleF();
                drawHelper.DrawRectangleBorder(spriteBatch, Color.Orange, rect.Location, rect.Size);
            }
        }
    }
}
