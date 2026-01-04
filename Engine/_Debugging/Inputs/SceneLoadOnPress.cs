using Microsoft.Xna.Framework.Input;

namespace Engine.Debugging;

public class SceneLoadOnPress : Component, IUpdatable
{
    public required ISceneDefinition SceneDefinition { get; init; }
    public Button Button { get; init; } = Keys.R;

    bool IUpdatable.Pause()
    {
        return false;
    }

    void IUpdatable.Update()
    {
        var inputManager = Scene.Singletons.Get<InputManager>();
        if (inputManager.IsPressed(Button))
        {
            Scene.Game.NextSceneDefinition = SceneDefinition;
        }
    }
}
