namespace Engine.Audio;

/// <summary>
/// Plays sounds even when paused. Disposes all sounds on unpause.
/// </summary>
public class MenuAudioManager : AudioManager
{
    protected override bool Pause()
    {
        return false;
    }

    protected override void Unpause()
    {
        StopAll();
    }
}
