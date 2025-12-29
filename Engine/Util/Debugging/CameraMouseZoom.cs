using Engine.Core;
using Engine.Graphics;
using Engine.Input;

namespace Engine.Util.Debugging;

public partial class CameraMouseZoom : Component
{
    private EntityHandle _cameraHandle;

    protected override void Begin()
    {
        _cameraHandle = new(Scene.Find<Camera>().First());
    }
}

public partial class CameraMouseZoom : IUpdatable
{
    void IUpdatable.Update()
    {
        var inputManager = Scene.Singletons.Get<InputManager>();
        var camera = _cameraHandle.Deref().Get<Camera>();
        var cameraTransform = camera.Entity.Get<Transform>();

        switch (inputManager.MouseWheelDelta)
        {
            case > 0:
                // TODO
                break;
            case < 0:
                // TODO
                break;
        }
    }
}
