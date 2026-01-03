using Engine;

namespace Library.Player;

public class CameraFollow : Component, IUpdatable
{
    private EntityHandle _cameraHandle;

    protected override void Begin()
    {
        _cameraHandle = new(Scene.Find<Camera>().First());
    }

    int IUpdatable.UpdateOrder => IUpdatable.UpdateOrderFrameEnd;

    void IUpdatable.Update()
    {
        var transform = Entity.Get<Transform>();
        var cameraTransform = _cameraHandle.Deref().Get<Transform>();
        cameraTransform.Position = transform.Position;
    }
}
