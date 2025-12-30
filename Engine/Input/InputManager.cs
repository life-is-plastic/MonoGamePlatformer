using System;
using Engine.Core;
using Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.Input;

public partial class InputManager : Component
{
    private readonly MouseInfo _mouseInfo = new();
    private KeyboardState _kbCurrentState = new();
    private KeyboardState _kbPreviousState = new();
    private EntityHandle _cameraHandle;

    public int MouseWheelDelta => _mouseInfo.CurrentWheelDelta;
    public Point MouseScreenPosition => _mouseInfo.CurrentMouseState.Position;
    public Vector2 MouseWorldPosition { get; private set; }

    protected override void Begin()
    {
        _cameraHandle = new(Scene.Find<Camera>().First());
    }

    private bool IsPreviouslyDown(Button button)
    {
        if (button.AsKey() is { } key)
        {
            return _kbPreviousState.IsKeyDown(key);
        }
        if (button.AsMouseButton() is { } mouseButton)
        {
            return _mouseInfo.IsPreviouslyDown(mouseButton);
        }
        throw new ArgumentException($"unknown button: {button}");
    }

    public bool IsDown(Button button)
    {
        if (button.AsKey() is { } key)
        {
            return _kbCurrentState.IsKeyDown(key);
        }
        if (button.AsMouseButton() is { } mouseButton)
        {
            return _mouseInfo.IsCurrentlyDown(mouseButton);
        }
        throw new ArgumentException($"unknown button: {button}");
    }

    public bool IsPressed(Button button)
    {
        return IsDown(button) && !IsPreviouslyDown(button);
    }

    public bool IsHeld(Button button)
    {
        return IsDown(button) && IsPreviouslyDown(button);
    }

    public bool IsReleased(Button button)
    {
        return !IsDown(button) && IsPreviouslyDown(button);
    }
}

public partial class InputManager : IUpdatable
{
    int IUpdatable.UpdateOrder => UpdateOrderInterval.FrameBegin[0];

    bool IUpdatable.Pause()
    {
        return false;
    }

    void IUpdatable.Update()
    {
        _kbPreviousState = _kbCurrentState;
        _kbCurrentState = Keyboard.GetState();

        _mouseInfo.Update();
        var camera = _cameraHandle.Deref().Get<Camera>();
        var cameraTransform = camera.Entity.Get<Transform>();
        var screenPosWrtScreenCenter =
            MouseScreenPosition.ToVector2() - Scene.Game.ViewportSize.ToVector2() / 2;
        MouseWorldPosition =
            cameraTransform.Position + screenPosWrtScreenCenter / camera.ScreenScale;
    }
}
