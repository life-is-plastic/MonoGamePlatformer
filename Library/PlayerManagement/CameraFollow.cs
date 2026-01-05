using Engine;
using Engine.Util;

namespace Library.PlayerManagement;

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
        cameraTransform.Position = cameraTransform.Position.SmoothStep(
            transform.Position,
            8 * Scene.DeltaTime
        );
    }
}
