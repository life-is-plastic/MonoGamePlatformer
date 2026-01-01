using System;
using Microsoft.Xna.Framework.Input;

namespace Engine;

public readonly record struct Button
{
    static Button()
    {
        ReadOnlySpan<Type> types = [typeof(Keys), typeof(MouseButton)];
        foreach (var type in types)
        {
            if (Enum.GetUnderlyingType(type) != typeof(int))
            {
                throw new DataMisalignedException($"{type} is not backed by {typeof(int)}");
            }
        }
    }

    public int Source { get; init; }
    public int Number { get; init; }

    public Keys? AsKey()
    {
        return Source == 0 ? (Keys)Number : null;
    }

    public MouseButton? AsMouseButton()
    {
        return Source == 1 ? (MouseButton)Number : null;
    }

    public static implicit operator Button(Keys key)
    {
        return new() { Source = 0, Number = (int)key };
    }

    public static implicit operator Button(MouseButton mouseButton)
    {
        return new() { Source = 1, Number = (int)mouseButton };
    }
}
