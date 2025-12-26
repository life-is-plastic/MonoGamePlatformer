using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Graphics;

public partial class Animation<T>
    where T : IDrawable
{
    private readonly T[] _drawables;
    private readonly float[] _durations;
    private int _currentIndex = -1;
    private float _timeUntilNextFrame;

    public (T Drawable, float Duration) this[int index] => (_drawables[index], _durations[index]);
    public int FrameCount => _drawables.Length;
    public int CurrentIndex
    {
        get => _currentIndex;
        set
        {
            Debug.Assert(value >= 0 && value < FrameCount);
            if (value != _currentIndex)
            {
                _currentIndex = value;
                _timeUntilNextFrame = _durations[_currentIndex];
            }
        }
    }

    public Animation(params ReadOnlySpan<(T Drawable, float Duration)> frames)
    {
        Debug.Assert(frames.Length > 0);
        _drawables = new T[frames.Length];
        _durations = new float[frames.Length];
        for (var i = 0; i < frames.Length; i++)
        {
            _drawables[i] = frames[i].Drawable;
            _durations[i] = frames[i].Duration;
            Debug.Assert(_durations[i] > 0);
        }
        CurrentIndex = 0;
    }

    /// <summary>
    /// Creates an animation from the given drawables where each frame has the same duration.
    /// </summary>
    public Animation(float duration, params ReadOnlySpan<T> drawables)
    {
        Debug.Assert(duration > 0);
        Debug.Assert(drawables.Length > 0);
        _drawables = drawables.ToArray();
        _durations = new float[drawables.Length];
        Array.Fill(_durations, duration);
        CurrentIndex = 0;
    }

    public void Update(float deltaTime)
    {
        _timeUntilNextFrame -= deltaTime;
        if (_timeUntilNextFrame <= 0)
        {
            _currentIndex++;
            _currentIndex %= FrameCount;
            _timeUntilNextFrame += _durations[_currentIndex];
        }
    }
}

public partial class Animation<T> : IDrawable
{
    public void Draw(
        SpriteBatch spriteBatch,
        Vector2 position,
        float rotation,
        Vector2 scale,
        SpriteEffects effects
    )
    {
        _drawables[_currentIndex].Draw(spriteBatch, position, rotation, scale, effects);
    }
}
