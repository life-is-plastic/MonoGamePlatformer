using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Engine.Core;

/// <summary>
/// Iterates over all entities with a component of type <c>T</c> (at the default component index),
/// yielding <c>(T, entity)</c> pairs.
/// </summary>
public readonly ref struct FindEntityEnumerable<T>
    where T : IComponent
{
    private readonly ReadOnlySpan<Entity> _entities;

    public FindEntityEnumerable(ReadOnlySpan<Entity> entities) => _entities = entities;

    public readonly Enumerator GetEnumerator() => new(_entities);

    public ref struct Enumerator(ReadOnlySpan<Entity> entities)
    {
        private FindEntityEnumeratorInternals _internals = new(entities, typeof(T));

        public readonly (T Component, Entity Entity) Current =>
            ((T)_internals.CurrentComponents[0], _internals.CurrentComponents[0].Entity);

        public bool MoveNext() => _internals.MoveNext();
    }
}

public readonly ref struct FindEntityEnumerable<T1, T2>
    where T1 : IComponent
    where T2 : IComponent
{
    private readonly ReadOnlySpan<Entity> _entities;

    public FindEntityEnumerable(ReadOnlySpan<Entity> entities) => _entities = entities;

    public readonly Enumerator GetEnumerator() => new(_entities);

    public ref struct Enumerator(ReadOnlySpan<Entity> entities)
    {
        private FindEntityEnumeratorInternals _internals = new(entities, typeof(T1), typeof(T2));

        public readonly (T1 Component1, T2 Component2, Entity Entity) Current =>
            (
                (T1)_internals.CurrentComponents[0],
                (T2)_internals.CurrentComponents[1],
                _internals.CurrentComponents[0].Entity
            );

        public bool MoveNext() => _internals.MoveNext();
    }
}

internal ref struct FindEntityEnumeratorInternals
{
    [InlineArray(4)]
    public struct Buffer<T>
    {
        private T _element0;
    }

    public readonly ReadOnlySpan<Entity> Entities;
    public readonly Buffer<Type> ComponentTypes;
    public Buffer<IComponent> CurrentComponents;
    public int NextEntity = 0;
    public readonly int ComponentCount;

    public FindEntityEnumeratorInternals(
        ReadOnlySpan<Entity> entities,
        params ReadOnlySpan<Type> componentTypes
    )
    {
        Debug.Assert(componentTypes.Length > 0);
        Entities = entities;
        componentTypes.CopyTo(ComponentTypes);
        ComponentCount = componentTypes.Length;
    }

    public bool MoveNext()
    {
        while (NextEntity < Entities.Length)
        {
            var entity = Entities[NextEntity++];
            for (var i = 0; i < ComponentCount; i++)
            {
                var type = ComponentTypes[i];
                if (entity.MaybeGet(type, Component.DefaultIndex) is { } component)
                {
                    CurrentComponents[i] = component;
                }
                else
                {
                    goto DiscardEntity;
                }
            }
            return true;

            DiscardEntity:
            ;
        }
        return false;
    }
}
