using Microsoft.Xna.Framework.Input;

namespace Engine.Debugging;

public class ScenePauseToggle : Component, IUpdatable
{
    public Button Button { get; init; } = Keys.Escape;

    bool IUpdatable.Pause()
    {
        return false;
    }

    void IUpdatable.Update()
    {
        var inputManager = Scene.Singletons.Get<InputManager>();
        if (inputManager.IsPressed(Button))
        {
            Scene.ShouldPause = !Scene.IsPaused;
        }
    }
}
