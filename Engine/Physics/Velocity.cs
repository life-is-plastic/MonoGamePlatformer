using Engine.Core;
using Microsoft.Xna.Framework;

namespace Engine.Physics;

public partial class Velocity : Component
{
    public Vector2 Linear;
    public float Angular;
}

public partial class Velocity : IUpdatable
{
    int IUpdatable.UpdateOrder => UpdateOrderInterval.Physics[^2];

    void IUpdatable.Update()
    {
        var transform = Entity.Get<Transform>();
        transform.Position += Linear * Scene.DeltaTime;
        transform.Rotation += Angular * Scene.DeltaTime;
    }
}
