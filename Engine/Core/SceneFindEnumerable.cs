using Engine.Util.Collections;

namespace Engine.Core;

/// <summary>
/// Iterates over all entities with a component of type <c>T</c> (at the default component index),
/// yielding <c>(T, entity)</c> pairs.
/// </summary>
public struct SceneFindEnumerable<T>(IndexedSetView<Entity> entities)
    where T : IComponent
{
    public readonly Enumerator GetEnumerator() => new(entities);

    public struct Enumerator(IndexedSetView<Entity> entities)
    {
        private SceneFindEnumerator _enumerator = new(entities, typeof(T));

        public readonly (T Component, Entity Entity) Current =>
            ((T)_enumerator.Current[0], _enumerator.Current[0].Entity);

        public bool MoveNext() => _enumerator.MoveNext();

        public readonly void Dispose() => _enumerator.Dispose();
    }
}

public struct SceneFindEnumerable<T1, T2>(IndexedSetView<Entity> entities)
    where T1 : IComponent
    where T2 : IComponent
{
    public readonly Enumerator GetEnumerator() => new(entities);

    public struct Enumerator(IndexedSetView<Entity> entities)
    {
        private SceneFindEnumerator _enumerator = new(entities, typeof(T1), typeof(T2));

        public readonly (T1 Component1, T2 Component2, Entity Entity) Current =>
            ((T1)_enumerator.Current[0], (T2)_enumerator.Current[1], _enumerator.Current[0].Entity);

        public bool MoveNext() => _enumerator.MoveNext();

        public readonly void Dispose() => _enumerator.Dispose();
    }
}

public struct SceneFindEnumerable<T1, T2, T3>(IndexedSetView<Entity> entities)
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
{
    public readonly Enumerator GetEnumerator() => new(entities);

    public struct Enumerator(IndexedSetView<Entity> entities)
    {
        private SceneFindEnumerator _enumerator = new(entities, typeof(T1), typeof(T2), typeof(T3));

        public readonly (T1 Component1, T2 Component2, T3 Component3, Entity Entity) Current =>
            (
                (T1)_enumerator.Current[0],
                (T2)_enumerator.Current[1],
                (T3)_enumerator.Current[2],
                _enumerator.Current[0].Entity
            );

        public bool MoveNext() => _enumerator.MoveNext();

        public readonly void Dispose() => _enumerator.Dispose();
    }
}

public struct SceneFindEnumerable<T1, T2, T3, T4>(IndexedSetView<Entity> entities)
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
{
    public readonly Enumerator GetEnumerator() => new(entities);

    public struct Enumerator(IndexedSetView<Entity> entities)
    {
        private SceneFindEnumerator _enumerator = new(
            entities,
            typeof(T1),
            typeof(T2),
            typeof(T3),
            typeof(T4)
        );

        public readonly (
            T1 Component1,
            T2 Component2,
            T3 Component3,
            T4 Component4,
            Entity Entity
        ) Current =>
            (
                (T1)_enumerator.Current[0],
                (T2)_enumerator.Current[1],
                (T3)_enumerator.Current[2],
                (T4)_enumerator.Current[3],
                _enumerator.Current[0].Entity
            );

        public bool MoveNext() => _enumerator.MoveNext();

        public readonly void Dispose() => _enumerator.Dispose();
    }
}
