using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Engine.EC;
using Engine.Graphics;
using Engine.Input;
using Engine.Util.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine.Physics;

/// <summary>
/// For debug collider visualization.
/// </summary>
public partial class ColliderRenderer : Component
{
    private readonly Keys _toggleKey;
    private Texture2D _pixel = null!;

    public ColliderRenderer(Keys toggleKey = Keys.None)
    {
        _toggleKey = toggleKey;
    }

    public override void Begin()
    {
        _pixel = new Texture2D(Scene.Game.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);
    }

    public override void End()
    {
        _pixel.Dispose();
    }

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_colliders")]
    private static extern ref Dictionary<int, IndexedSet<Collider>> GetColliders(
        CollisionManager collisionManager
    );
}

public partial class ColliderRenderer : IUpdatable
{
    bool IUpdatable.Pause()
    {
        return false;
    }

    void IUpdatable.Update()
    {
        var inputManager = GetSingleton<InputManager>();
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
        var collisionManager = GetSingleton<CollisionManager>();
        foreach (var colliders in GetColliders(collisionManager).Values)
        {
            foreach (var collider in colliders)
            {
                var rect = collider.AsWorldRect().ToRectangle();
                spriteBatch.Draw(
                    _pixel,
                    new Rectangle(rect.Left, rect.Top, rect.Width, 1),
                    Color.Orange
                );
                spriteBatch.Draw(
                    _pixel,
                    new Rectangle(rect.Left, rect.Top, 1, rect.Height),
                    Color.Orange
                );
                spriteBatch.Draw(
                    _pixel,
                    new Rectangle(rect.Left, rect.Bottom, rect.Width, 1),
                    Color.Orange
                );
                spriteBatch.Draw(
                    _pixel,
                    new Rectangle(rect.Right, rect.Top, 1, rect.Height),
                    Color.Orange
                );
            }
        }
    }
}
