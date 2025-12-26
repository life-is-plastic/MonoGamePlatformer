using Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.Input;

public partial class InputManager : Component { }

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
        _kbPreviousState = _kbCurrentState;
        _kbCurrentState = Keyboard.GetState();

        _mouseInfo.Update();
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
}

public partial class InputManager
{
    private readonly MouseInfo _mouseInfo = new();

    public Point MouseScreenPosition => _mouseInfo.CurrentMouseState.Position;
    public int MouseWheelDelta => _mouseInfo.CurrentWheelDelta;

    public bool IsDown(MouseButton button) => _mouseInfo.IsCurrentlyDown(button);

    public bool IsUp(MouseButton button) => !IsDown(button);

    public bool IsPressed(MouseButton button) =>
        IsDown(button) && !_mouseInfo.IsPreviouslyDown(button);

    public bool IsHeld(MouseButton button) => IsDown(button) && _mouseInfo.IsPreviouslyDown(button);

    public bool IsReleased(MouseButton button) =>
        IsUp(button) && _mouseInfo.IsPreviouslyDown(button);
}
