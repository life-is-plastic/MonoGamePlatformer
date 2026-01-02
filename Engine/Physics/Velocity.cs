using Microsoft.Xna.Framework;

namespace Engine;

public class Velocity : Component, IUpdatable
{
    public Vector2 Linear = new();
    public float Angular = 0;

    int IUpdatable.UpdateOrder => IUpdatable.UpdateOrderPhysics[^2];

    void IUpdatable.Update()
    {
        var transform = Entity.Get<Transform>();
        transform.Position += Linear * Scene.DeltaTime;
        transform.Rotation += Angular * Scene.DeltaTime;
    }
}
