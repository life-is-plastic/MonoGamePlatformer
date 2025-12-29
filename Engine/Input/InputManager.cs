using Engine.Core;
using Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.Input;

public partial class InputManager : Component
{
    private EntityHandle _cameraHandle;

    protected override void Begin()
    {
        _cameraHandle = new(Scene.Find<Camera>().First());
    }
}

public partial class InputManager : IUpdatable
{
    public const int UpdateOrder = int.MinValue;
    int IUpdatable.UpdateOrder => UpdateOrder;

    bool IUpdatable.Pause()
    {
        return false;
    }

    void IUpdatable.Update()
    {
        UpdateKeyboard();
        UpdateMouse();
    }
}

public partial class InputManager
{
    private KeyboardState _kbCurrentState = new();
    private KeyboardState _kbPreviousState = new();

    public bool IsDown(Keys button) => _kbCurrentState.IsKeyDown(button);

    public bool IsUp(Keys button) => _kbCurrentState.IsKeyUp(button);

    public bool IsPressed(Keys button) => IsDown(button) && _kbPreviousState.IsKeyUp(button);

    public bool IsHeld(Keys button) => IsDown(button) && _kbPreviousState.IsKeyDown(button);

    public bool IsReleased(Keys button) => IsUp(button) && _kbPreviousState.IsKeyDown(button);

    private void UpdateKeyboard()
    {
        _kbPreviousState = _kbCurrentState;
        _kbCurrentState = Keyboard.GetState();
    }
}

public partial class InputManager
{
    private readonly MouseInfo _mouseInfo = new();

    public int MouseWheelDelta => _mouseInfo.CurrentWheelDelta;
    public Point MouseScreenPosition => _mouseInfo.CurrentMouseState.Position;
    public Vector2 MouseWorldPosition { get; private set; }

    public Vector2 GetMouseWorldPosition()
    {
        var camera = _cameraHandle.Deref().Get<Camera>();
        var cameraTransform = camera.Entity.Get<Transform>();

        var mouseScreenPositionRelCenter =
            MouseScreenPosition.ToVector2() - Scene.Game.ViewportSize.ToVector2() / 2;
        return cameraTransform.Position + mouseScreenPositionRelCenter / camera.ScreenScale;
    }

    public bool IsDown(MouseButton button) => _mouseInfo.IsCurrentlyDown(button);

    public bool IsUp(MouseButton button) => !IsDown(button);

    public bool IsPressed(MouseButton button) =>
        IsDown(button) && !_mouseInfo.IsPreviouslyDown(button);

    public bool IsHeld(MouseButton button) => IsDown(button) && _mouseInfo.IsPreviouslyDown(button);

    public bool IsReleased(MouseButton button) =>
        IsUp(button) && _mouseInfo.IsPreviouslyDown(button);

    private void UpdateMouse()
    {
        _mouseInfo.Update();

        var camera = _cameraHandle.Deref().Get<Camera>();
        var cameraTransform = camera.Entity.Get<Transform>();
        var screenPosWrtScreenCenter =
            MouseScreenPosition.ToVector2() - Scene.Game.ViewportSize.ToVector2() / 2;
        MouseWorldPosition =
            cameraTransform.Position + screenPosWrtScreenCenter / camera.ScreenScale;
    }
}
