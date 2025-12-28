using System;
using System.Runtime.CompilerServices;

namespace Engine.Core;

/// <summary>
/// Yields all entities with the given component types (all at the default component index).
/// </summary>
public readonly ref struct FindEntityEnumerable
{
    private readonly ReadOnlySpan<Entity> _entities;
    private readonly Buffer<Type> _componentTypes;
    private readonly int _componentCount;

    public FindEntityEnumerable(
        ReadOnlySpan<Entity> entities,
        params ReadOnlySpan<Type> componentTypes
    )
    {
        _entities = entities;
        componentTypes.CopyTo(_componentTypes);
        _componentCount = componentTypes.Length;
    }

    public readonly Enumerator GetEnumerator()
    {
        return new(_entities, _componentTypes[.._componentCount]);
    }

    public readonly Entity First()
    {
        return MaybeFirst()
            ?? throw new InvalidOperationException(
                $"no entity found with all of [{string.Join(", ", _componentTypes[.._componentCount])}]"
            );
    }

    public readonly Entity? MaybeFirst()
    {
        foreach (var entity in this)
        {
            return entity;
        }
        return null;
    }

    [InlineArray(4)]
    private struct Buffer<T>
    {
        private T _element0;
    }

    public ref struct Enumerator
    {
        private readonly ReadOnlySpan<Entity> _entities;
        private readonly Buffer<Type> _componentTypes;
        private readonly int _componentCount;
        private int _nextEntity = 0;

        public Entity Current { get; private set; } = null!;

        public Enumerator(ReadOnlySpan<Entity> entities, params ReadOnlySpan<Type> componentTypes)
        {
            _entities = entities;
            componentTypes.CopyTo(_componentTypes);
            _componentCount = componentTypes.Length;
        }

        public bool MoveNext()
        {
            while (_nextEntity < _entities.Length)
            {
                var entity = _entities[_nextEntity++];
                for (var i = 0; i < _componentCount; i++)
                {
                    if (!entity.Has(_componentTypes[i], Component.DefaultIndex))
                    {
                        goto DiscardEntity;
                    }
                }
                Current = entity;
                return true;

                DiscardEntity:
                ;
            }
            return false;
        }
    }
}
