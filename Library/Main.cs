using Engine.App;
using Engine.Audio;
using Library.Scenes;

namespace Library;

public class Main
{
    public static void Run()
    {
        AudioManager.MasterVolume = 0.4f;
        new Game(new DevScene()).Run();
    }
}
