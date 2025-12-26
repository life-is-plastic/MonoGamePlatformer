using Engine.Core;
using Microsoft.Xna.Framework;

namespace Engine.Physics;

public partial class Velocity : Component
{
    public Vector2 Linear;
}

public partial class Velocity : IUpdatable
{
    public const int UpdateOrder = CollisionManager.UpdateOrder - 1;
    int IUpdatable.UpdateOrder => UpdateOrder;

    void IUpdatable.Update()
    {
        Entity.Get<Transform>().Position += Linear * Scene.DeltaTime;
    }
}
