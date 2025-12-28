using Engine.Core;
using Engine.Input;
using Microsoft.Xna.Framework.Input;

namespace Engine.Util.Debugging;

public partial class ScenePauseToggle : Component
{
    private readonly Keys _button;

    public ScenePauseToggle(Keys button = Keys.Escape)
    {
        _button = button;
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
