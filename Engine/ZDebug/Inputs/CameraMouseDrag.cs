using Engine.Core;
using Engine.Graphics;
using Engine.Input;
using Microsoft.Xna.Framework;

namespace Engine.ZDebug.Inputs;

public partial class CameraMouseDrag : Component
{
    private readonly Button _button;
    private EntityHandle _cameraHandle;
    private bool _dragging = false;
    private Point _initialScreenPos = default;
    private Vector2 _initialWorldPos = default;

    public CameraMouseDrag(Button? button = null)
    {
        _button = button ?? MouseButton.Right;
    }

    protected override void Begin()
    {
        _cameraHandle = new(Scene.Find<Camera>().First());
    }
}

public partial class CameraMouseDrag : IUpdatable
{
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
            if (inputManager.IsPressed(_button))
            {
                _initialScreenPos = inputManager.MouseScreenPosition;
                _initialWorldPos = cameraTransform.Position;
                _dragging = true;
            }
            return;
        }

        if (!inputManager.IsDown(_button))
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
