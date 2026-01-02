using Microsoft.Xna.Framework.Input;

namespace Engine.Debugging;

public class ScenePauseToggle : Component, IUpdatable
{
    private readonly Button _button;

    public ScenePauseToggle(Button? button = null)
    {
        _button = button ?? Keys.Escape;
    }

    bool IUpdatable.Pause()
    {
        return false;
    }

    void IUpdatable.Update()
    {
        var inputManager = Scene.Singletons.Get<InputManager>();
        if (inputManager.IsPressed(_button))
        {
            Scene.ShouldPause = !Scene.IsPaused;
        }
    }
}
