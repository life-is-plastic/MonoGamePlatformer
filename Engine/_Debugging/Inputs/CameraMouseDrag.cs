using Microsoft.Xna.Framework;

namespace Engine.Debugging;

public class CameraMouseDrag : Component, IUpdatable
{
    private EntityHandle _cameraHandle;
    private bool _dragging = false;
    private Point _initialScreenPos = new();
    private Vector2 _initialWorldPos = new();

    public Button Button { get; init; } = MouseButton.Right;

    protected override void Begin()
    {
        _cameraHandle = new(Scene.Find<Camera>().First());
    }

    bool IUpdatable.Pause()
    {
        _dragging = false;
        return true;
    }

    void IUpdatable.Update()
    {
        var inputManager = Scene.Singletons.Get<InputManager>();
        var camera = _cameraHandle.Deref().Get<Camera>();
        var cameraTransform = camera.Entity.Get<Transform>();

        if (!_dragging)
        {
            if (inputManager.IsPressed(Button))
            {
                _initialScreenPos = inputManager.MouseScreenPosition;
                _initialWorldPos = cameraTransform.Position;
                _dragging = true;
            }
            return;
        }

        if (!inputManager.IsDown(Button))
        {
            _dragging = false;
            return;
        }

        var deltaScreenPos =
            inputManager.MouseScreenPosition.ToVector2() - _initialScreenPos.ToVector2();
        var deltaWorldPos = -deltaScreenPos / (camera.ScreenScale * cameraTransform.Scale);
        cameraTransform.Position = _initialWorldPos + deltaWorldPos;
    }
}
