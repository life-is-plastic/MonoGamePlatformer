using System;
using Microsoft.Xna.Framework.Input;

namespace Engine.Input;

public class MouseInfo
{
    public MouseState CurrentMouseState { get; private set; } = new();
    public MouseState PreviousMouseState { get; private set; } = new();
    public int CurrentWheelDelta =>
        CurrentMouseState.ScrollWheelValue - PreviousMouseState.ScrollWheelValue;
    public int PreviousWheelDelta { get; private set; } = 0;

    private static bool IsDown(MouseState mouseState, int wheelDelta, MouseButton button)
    {
        return button switch
        {
            MouseButton.Left => mouseState.LeftButton == ButtonState.Pressed,
            MouseButton.Right => mouseState.RightButton == ButtonState.Pressed,
            MouseButton.Middle => mouseState.MiddleButton == ButtonState.Pressed,
            MouseButton.X1 => mouseState.XButton1 == ButtonState.Pressed,
            MouseButton.X2 => mouseState.XButton2 == ButtonState.Pressed,
            MouseButton.WheelUp => wheelDelta > 0,
            MouseButton.WheelDown => wheelDelta < 0,
            _ => throw new NotImplementedException(),
        };
    }

    public bool IsCurrentlyDown(MouseButton button) =>
        IsDown(CurrentMouseState, CurrentWheelDelta, button);

    public bool IsPreviouslyDown(MouseButton button) =>
        IsDown(PreviousMouseState, PreviousWheelDelta, button);

    public void Update()
    {
        PreviousWheelDelta = CurrentWheelDelta;
        PreviousMouseState = CurrentMouseState;
        CurrentMouseState = Mouse.GetState();
    }
}
