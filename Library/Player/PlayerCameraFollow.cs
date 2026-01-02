using Engine;

namespace Library;

public class PlayerCameraFollow : Component, IUpdatable
{
    private EntityHandle _playerHandle;
    private EntityHandle _cameraHandle;

    protected override void Begin()
    {
        _playerHandle = new(Scene.Find<Player>().First());
        _cameraHandle = new(Scene.Find<Camera>().First());
    }

    int IUpdatable.UpdateOrder => IUpdatable.UpdateOrderFrameEnd;

    void IUpdatable.Update()
    {
        var playerTransform = _playerHandle.Deref().Get<Transform>();
        var cameraTransform = _cameraHandle.Deref().Get<Transform>();
        cameraTransform.Position = playerTransform.Position;
    }
}
