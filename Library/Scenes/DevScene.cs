using Engine.Audio;
using Engine.Core;
using Engine.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace Library.Scenes;

public class DevScene : Scene
{
    public override string Name => nameof(DevScene);

    public override void Initialize()
    {
        Singletons.StageAttach(new DevSceneController());

        Singletons.Get<AudioManager>().Play(Content.Load<SoundEffect>("Audio/Theme"), loop: true);
    }
}

internal class DevSceneController : Component, IUpdatable
{
    bool IUpdatable.Pause()
    {
        return false;
    }

    void IUpdatable.Update()
    {
        var inputManager = GetSingleton<InputManager>();
        if (inputManager.IsPressed(Keys.Escape))
        {
            Scene.ShouldPause = !Scene.IsPaused;
        }
    }
}
