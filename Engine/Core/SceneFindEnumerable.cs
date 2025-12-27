using Engine.Util.Collections;

namespace Engine.Core;

/// <summary>
/// Iterates over all entities with a component of type <c>T</c> (at the default component index),
/// yielding <c>(T, entity)</c> pairs.
/// </summary>
public readonly struct SceneFindEnumerable<T>
    where T : IComponent
{
    private readonly IndexedSetView<Entity> _entities;

    public SceneFindEnumerable(in IndexedSetView<Entity> entities)
    {
        _entities = entities;
    }

    public readonly Enumerator GetEnumerator()
    {
        return new Enumerator(_entities);
    }

    public struct Enumerator
    {
        private SceneFindEnumerator _enumerator;

        public readonly (T Component, Entity Entity) Current =>
            ((T)_enumerator.Current[0], _enumerator.Current[0].Entity);

        public Enumerator(in IndexedSetView<Entity> entities)
        {
            _enumerator = new(entities, typeof(T));
        }

        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        public readonly void Dispose()
        {
            _enumerator.Dispose();
        }
    }
}

public readonly struct SceneFindEnumerable<T1, T2>
    where T1 : IComponent
    where T2 : IComponent
{
    private readonly IndexedSetView<Entity> _entities;

    public SceneFindEnumerable(in IndexedSetView<Entity> entities)
    {
        _entities = entities;
    }

    public readonly Enumerator GetEnumerator()
    {
        return new Enumerator(_entities);
    }

    public struct Enumerator
    {
        private SceneFindEnumerator _enumerator;

        public readonly (T1 Component1, T2 Component2, Entity Entity) Current =>
            ((T1)_enumerator.Current[0], (T2)_enumerator.Current[1], _enumerator.Current[0].Entity);

        public Enumerator(in IndexedSetView<Entity> entities)
        {
            _enumerator = new(entities, typeof(T1), typeof(T2));
        }

        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        public readonly void Dispose()
        {
            _enumerator.Dispose();
        }
    }
}
