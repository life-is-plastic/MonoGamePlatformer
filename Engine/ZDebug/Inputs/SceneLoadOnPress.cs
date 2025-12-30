using Engine.Core;
using Engine.Input;
using Microsoft.Xna.Framework.Input;

namespace Engine.ZDebug.Inputs;

public partial class SceneLoadOnPress : Component
{
    private readonly ISceneDefinition _sceneDefinition;
    private readonly Button _button;

    public SceneLoadOnPress(ISceneDefinition sceneDefinition, Button? button = null)
    {
        _sceneDefinition = sceneDefinition;
        _button = button ?? Keys.R;
    }
}

public partial class SceneLoadOnPress : IUpdatable
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
            Scene.Game.NextSceneDefinition = _sceneDefinition;
        }
    }
}
