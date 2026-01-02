using Microsoft.Xna.Framework.Input;

namespace Engine.Debugging;

public class SceneLoadOnPress : Component, IUpdatable
{
    private readonly ISceneDefinition _sceneDefinition;
    private readonly Button _button;

    public SceneLoadOnPress(ISceneDefinition sceneDefinition, Button? button = null)
    {
        _sceneDefinition = sceneDefinition;
        _button = button ?? Keys.R;
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
            Scene.Game.NextSceneDefinition = _sceneDefinition;
        }
    }
}
