using System.Collections;
using System.Collections.Generic;

namespace Engine.Util.Collections;

/// <summary>
/// A read-only view of an <c>IndexedSet</c>.
/// </summary>
public readonly partial record struct IndexedSetView<T>
    where T : notnull
{
    public static IndexedSetView<T> Empty { get; } = new(new());

    private readonly IndexedSet<T> _indexedSet;

    public int Count => _indexedSet.Count;
    public T this[int index] => _indexedSet[index];

    public IndexedSetView(in IndexedSet<T> indexedSet)
    {
        _indexedSet = indexedSet;
    }

    public bool Contains(in T item)
    {
        return _indexedSet.Contains(item);
    }
}

public readonly partial record struct IndexedSetView<T> : IEnumerable<T>
{
    public List<T>.Enumerator GetEnumerator()
    {
        return _indexedSet.GetEnumerator();
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
