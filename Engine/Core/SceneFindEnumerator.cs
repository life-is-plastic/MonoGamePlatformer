using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Engine.Core;

public ref struct SceneFindEnumerator
{
    private const int BufferLength = 4;

    [InlineArray(BufferLength)]
    public struct Buffer<T>
    {
        private T _element0;
    }

    public Buffer<IComponent> Current;

    private readonly ReadOnlySpan<Entity> _entities;
    private readonly int _bufCount;
    private Buffer<Type> _types;
    private int _nextEntity = 0;

    public SceneFindEnumerator(
        ReadOnlySpan<Entity> entities,
        params ReadOnlySpan<Type> componentTypes
    )
    {
        Debug.Assert(componentTypes.Length > 0 && componentTypes.Length < BufferLength);
        _entities = entities;
        _bufCount = componentTypes.Length;
        for (var i = 0; i < _bufCount; i++)
        {
            _types[i] = componentTypes[i];
        }
    }

    public bool MoveNext()
    {
        for (var i = _nextEntity; i < _entities.Length; i++)
        {
            var entity = _entities[i];
            for (var j = 0; j < _bufCount; j++)
            {
                var type = _types[j];
                if (entity.MaybeGet(type, Component.DefaultIndex) is { } component)
                {
                    Current[j] = component;
                }
                else
                {
                    goto NextEntity;
                }
            }
            _nextEntity = i + 1;
            return true;

            NextEntity:
            ;
        }
        return false;
    }
}
