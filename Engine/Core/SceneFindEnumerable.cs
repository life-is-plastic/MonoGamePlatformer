using System;

namespace Engine.Core;

/// <summary>
/// Iterates over all entities with a component of type <c>T</c> (at the default component index),
/// yielding <c>(T, entity)</c> pairs.
/// </summary>
public readonly ref struct SceneFindEnumerable<T>
    where T : IComponent
{
    private readonly ReadOnlySpan<Entity> _entities;

    public SceneFindEnumerable(ReadOnlySpan<Entity> entities) => _entities = entities;

    public readonly Enumerator GetEnumerator() => new(_entities);

    public ref struct Enumerator(ReadOnlySpan<Entity> entities)
    {
        private SceneFindEnumerator _enumerator = new(entities, typeof(T));

        public readonly (T Component, Entity Entity) Current =>
            ((T)_enumerator.Current[0], _enumerator.Current[0].Entity);

        public bool MoveNext() => _enumerator.MoveNext();
    }
}

public readonly ref struct SceneFindEnumerable<T1, T2>
    where T1 : IComponent
    where T2 : IComponent
{
    private readonly ReadOnlySpan<Entity> _entities;

    public SceneFindEnumerable(ReadOnlySpan<Entity> entities) => _entities = entities;

    public readonly Enumerator GetEnumerator() => new(_entities);

    public ref struct Enumerator(ReadOnlySpan<Entity> entities)
    {
        private SceneFindEnumerator _enumerator = new(entities, typeof(T1), typeof(T2));

        public readonly (T1 Component1, T2 Component2, Entity Entity) Current =>
            ((T1)_enumerator.Current[0], (T2)_enumerator.Current[1], _enumerator.Current[0].Entity);

        public bool MoveNext() => _enumerator.MoveNext();
    }
}
