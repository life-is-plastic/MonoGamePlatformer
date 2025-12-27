using System;
using System.Buffers;
using System.Diagnostics;
using Engine.Util.Collections;

namespace Engine.EC;

public struct SceneFindEnumerator
{
    private readonly IndexedSetView<Entity> _entities;
    private readonly int _componentCount;
    private readonly Type[] _componentTypes;
    private readonly IComponent[] _componentBuf;
    private int _nextEntity = 0;

    public readonly ReadOnlySpan<IComponent> Current => _componentBuf.AsSpan()[.._componentCount];

    public SceneFindEnumerator(
        in IndexedSetView<Entity> entities,
        params ReadOnlySpan<Type> componentTypes
    )
    {
        Debug.Assert(componentTypes.Length > 0);
        _entities = entities;
        _componentCount = componentTypes.Length;
        _componentTypes = ArrayPool<Type>.Shared.Rent(_componentCount);
        _componentBuf = ArrayPool<IComponent>.Shared.Rent(_componentCount);
        for (var i = 0; i < _componentCount; i++)
        {
            Debug.Assert(componentTypes[i].IsAssignableTo(typeof(IComponent)));
            _componentTypes[i] = componentTypes[i];
        }
    }

    public bool MoveNext()
    {
        for (var i = _nextEntity; i < _entities.Count; i++)
        {
            var entity = _entities[i];
            for (var j = 0; j < _componentCount; j++)
            {
                if (entity.MaybeGet(_componentTypes[j], Component.DefaultIndex) is { } component)
                {
                    _componentBuf[j] = component;
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

    public readonly void Dispose()
    {
        ArrayPool<Type>.Shared.Return(_componentTypes);
        ArrayPool<IComponent>.Shared.Return(_componentBuf);
    }
}
