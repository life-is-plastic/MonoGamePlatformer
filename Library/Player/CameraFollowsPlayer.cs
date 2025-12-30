using Engine.Core;
using Engine.Graphics;

namespace Library.Player;

public partial class CameraFollowsPlayer : Component
{
    private EntityHandle _playerHandle;
    private EntityHandle _cameraHandle;

    protected override void Begin()
    {
        _playerHandle = new(Scene.Find<PlayerMarker>().First());
        _cameraHandle = new(Scene.Find<Camera>().First());
    }
}

public partial class CameraFollowsPlayer : IUpdatable
{
    int IUpdatable.UpdateOrder => IUpdatable.UpdateOrderFrameEnd;

    void IUpdatable.Update()
    {
        _cameraHandle.Deref().Get<Transform>().Position = _playerHandle
            .Deref()
            .Get<Transform>()
            .Position;
    }
}
