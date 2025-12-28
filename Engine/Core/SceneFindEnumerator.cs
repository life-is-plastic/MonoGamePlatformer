using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Engine.Core;

public ref struct SceneFindEnumerator
{
    private static readonly ThreadLocal<List<IComponent>> s_componentBuf = new(() => new(4));
    private static readonly ThreadLocal<List<Type>> s_typeBuf = new(() => new(4));

    private readonly ReadOnlySpan<Entity> _entities;
    private readonly List<IComponent> _componentBuf;
    private readonly List<Type> _typeBuf;
    private int _nextEntity = 0;

    public readonly ReadOnlySpan<IComponent> Current => CollectionsMarshal.AsSpan(_componentBuf);

    public SceneFindEnumerator(
        ReadOnlySpan<Entity> entities,
        params ReadOnlySpan<Type> componentTypes
    )
    {
        Debug.Assert(componentTypes.Length > 0);
        _entities = entities;
        _componentBuf = s_componentBuf.Value!;
        _componentBuf.Clear();
        _typeBuf = s_typeBuf.Value!;
        _typeBuf.Clear();
        _typeBuf.AddRange(componentTypes);
    }

    public bool MoveNext()
    {
        for (var i = _nextEntity; i < _entities.Length; i++)
        {
            _componentBuf.Clear();
            var entity = _entities[i];
            foreach (var type in _typeBuf)
            {
                if (entity.MaybeGet(type, Component.DefaultIndex) is { } component)
                {
                    _componentBuf.Add(component);
                }
                else
                {
                    _componentBuf.Clear();
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
