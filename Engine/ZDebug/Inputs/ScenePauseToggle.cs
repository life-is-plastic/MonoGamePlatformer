using Engine.Core;
using Engine.Input;
using Microsoft.Xna.Framework.Input;

namespace Engine.ZDebug.Inputs;

public partial class ScenePauseToggle : Component
{
    private readonly Button _button;

    public ScenePauseToggle(Button? button = null)
    {
        _button = button ?? Keys.Escape;
    }
}

public partial class ScenePauseToggle : IUpdatable
{
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
