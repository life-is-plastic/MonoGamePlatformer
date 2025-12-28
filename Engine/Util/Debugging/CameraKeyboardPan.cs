using Engine.Core;
using Engine.Graphics;
using Engine.Input;
using Microsoft.Xna.Framework.Input;

namespace Engine.Util.Debugging;

public partial class CameraKeyboardPan : Component
{
    private readonly float _panSpeed;
    private readonly Keys _left;
    private readonly Keys _right;
    private readonly Keys _up;
    private readonly Keys _down;
    private EntityHandle _cameraHandle;

    public CameraKeyboardPan(
        float panSpeed = 320,
        Keys left = Keys.A,
        Keys right = Keys.D,
        Keys up = Keys.W,
        Keys down = Keys.S
    )
    {
        _panSpeed = panSpeed;
        _left = left;
        _right = right;
        _up = up;
        _down = down;
    }

    protected override void Begin()
    {
        _cameraHandle = new(Scene.Find<Camera>().First());
    }
}

public partial class CameraKeyboardPan : IUpdatable
{
    void IUpdatable.Update()
    {
        var inputManager = Scene.Singletons.Get<InputManager>();
        var transform = _cameraHandle.Deref().Get<Transform>();
        var displacement = _panSpeed * Scene.DeltaTime;
        if (inputManager.IsDown(_left))
        {
            transform.Position.X -= displacement;
        }
        if (inputManager.IsDown(_right))
        {
            transform.Position.X += displacement;
        }
        if (inputManager.IsDown(_up))
        {
            transform.Position.Y -= displacement;
        }
        if (inputManager.IsDown(_down))
        {
            transform.Position.Y += displacement;
        }
    }
}
