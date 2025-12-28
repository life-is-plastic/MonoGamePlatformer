using Engine.Core;
using Engine.Graphics;
using Engine.Input;
using Microsoft.Xna.Framework;

namespace Engine.Util.Debugging;

public partial class MouseDragsCamera : Component
{
    private readonly MouseButton _button;
    private EntityHandle _cameraHandle;
    private bool _dragging = false;
    private Point _initialScreenPos;
    private Vector2 _initialWorldPos;

    public MouseDragsCamera(MouseButton button = MouseButton.Right)
    {
        _button = button;
    }

    protected override void Begin()
    {
        _cameraHandle = new(Scene.Find<Camera>().First());
    }
}

public partial class MouseDragsCamera : IUpdatable
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

        if (inputManager.IsUp(_button))
        {
            _dragging = false;
            return;
        }

        cameraTransform.Position =
            _initialWorldPos
            - (inputManager.MouseScreenPosition.ToVector2() - _initialScreenPos.ToVector2())
                / camera.ViewportScale;
    }
}
