using System;
using System.Collections.Generic;
using Engine.EC;
using Microsoft.Xna.Framework.Audio;

namespace Engine.Audio;

public partial class AudioManager : Component
{
    public static float MasterVolume
    {
        get;
        set => field = Math.Clamp(value, 0, 1);
    } = 1;

    private readonly List<SoundEffectInstance> _active = new();

    public void Play(SoundEffect soundEffect, float volume = 1, bool loop = false)
    {
        var instance = soundEffect.CreateInstance();
        instance.Volume = MasterVolume * Math.Clamp(volume, 0, 1);
        instance.IsLooped = loop;
        instance.Play();
        _active.Add(instance);
    }

    public void StopAll()
    {
        foreach (var instance in _active)
        {
            instance.Dispose();
        }
        _active.Clear();
    }

    public override void End()
    {
        StopAll();
    }
}

public partial class AudioManager : IUpdatable
{
    protected virtual bool Pause()
    {
        foreach (var instance in _active)
        {
            instance.Pause();
        }
        return true;
    }

    bool IUpdatable.Pause()
    {
        return Pause();
    }

    protected virtual void Unpause()
    {
        foreach (var instance in _active)
        {
            instance.Resume();
        }
    }

    void IUpdatable.Unpause()
    {
        Unpause();
    }

    void IUpdatable.Update()
    {
        for (var i = _active.Count - 1; i >= 0; i--)
        {
            var instance = _active[i];
            if (instance.State == SoundState.Stopped)
            {
                instance.Dispose();
                _active.RemoveAt(i);
            }
        }
    }
}
