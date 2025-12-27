using System;
using System.Collections.Generic;

namespace Engine.EC;

/// <summary>
/// Container for a cached component reference.
/// </summary>
public readonly struct ComponentHandle<T>
    where T : IComponent
{
    private readonly T _component;

    public ComponentHandle(T component)
    {
        _component = component;
    }

    /// <summary>
    /// Returns the wrapped component, throwing an exception if the component is no longer part of
    /// the scene.
    /// </summary>
    public T Deref()
    {
        return MaybeDeref()
            ?? throw new InvalidOperationException(
                $"{_component} is no longer part of {_component.Entity.Scene}"
            );
    }

    /// <summary>
    /// Returns the wrapped component, or null if the component is no longer part of the scene.
    /// </summary>
    public T? MaybeDeref()
    {
        if (
            EqualityComparer<T>.Default.Equals(
                _component,
                _component.Entity.MaybeGet<T>(_component.ComponentIndex)
            )
        )
        {
            return _component;
        }
        return default;
    }
}
