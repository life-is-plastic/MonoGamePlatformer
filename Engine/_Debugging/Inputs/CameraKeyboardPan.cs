using Microsoft.Xna.Framework.Input;

namespace Engine.Debugging;

public class CameraButtonPan : Component, IUpdatable
{
    private EntityHandle _cameraHandle;

    public Button Left { get; init; } = Keys.A;
    public Button Right { get; init; } = Keys.D;
    public Button Up { get; init; } = Keys.W;
    public Button Down { get; init; } = Keys.S;
    public float PanSpeed { get; init; } = 320;

    protected override void Begin()
    {
        _cameraHandle = new(Scene.Find<Camera>().First());
    }

    void IUpdatable.Update()
    {
        var inputManager = Scene.Singletons.Get<InputManager>();
        var transform = _cameraHandle.Deref().Get<Transform>();
        var displacement = PanSpeed * Scene.DeltaTime;
        if (inputManager.IsDown(Left))
        {
            transform.Position.X -= displacement;
        }
        if (inputManager.IsDown(Right))
        {
            transform.Position.X += displacement;
        }
        if (inputManager.IsDown(Up))
        {
            transform.Position.Y -= displacement;
        }
        if (inputManager.IsDown(Down))
        {
            transform.Position.Y += displacement;
        }
    }
}
