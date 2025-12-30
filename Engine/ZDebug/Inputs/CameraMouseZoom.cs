using Engine.Core;
using Engine.Graphics;
using Engine.Input;

namespace Engine.ZDebug.Inputs;

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
        var cameraTransform = _cameraHandle.Deref().Get<Transform>();
        var mult = 1.25f;
        switch (inputManager.MouseWheelDelta)
        {
            case > 0:
                cameraTransform.Scale *= mult;
                break;
            case < 0:
                cameraTransform.Scale /= mult;
                break;
        }
    }
}
