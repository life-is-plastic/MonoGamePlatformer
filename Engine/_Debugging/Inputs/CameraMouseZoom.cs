namespace Engine.Debugging;

public class CameraMouseZoom : Component, IUpdatable
{
    private EntityHandle _cameraHandle;

    protected override void Begin()
    {
        _cameraHandle = new(Scene.Find<Camera>().First());
    }

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
