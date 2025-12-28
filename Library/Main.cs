using Engine.Audio;
using Engine.Core;
using Library.Scenes;

namespace Library;

public class Main
{
    public static void Run()
    {
        AudioManager.MasterVolume = 0.2f;
        new Game(DefaultSceneDefinition.Instance).Run();
    }
}
