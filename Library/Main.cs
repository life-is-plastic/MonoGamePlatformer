using Engine.Audio;
using Engine.Core;
using Engine.ZDebug.Scenes;

namespace Library;

public class Main
{
    public static void Run()
    {
        AudioManager.MasterVolume = 0.2f;
        new Game(PhysicsSandboxScene.Instance).Run();
    }
}
