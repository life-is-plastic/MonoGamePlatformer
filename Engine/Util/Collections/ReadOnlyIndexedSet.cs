using System.Collections;
using System.Collections.Generic;

namespace Engine.Util.Collections;

/// <summary>
/// A read-only view over an <c>IndexedSet</c>.
/// </summary>
public readonly partial record struct ReadOnlyIndexedSet<T>
    where T : notnull
{
    public static ReadOnlyIndexedSet<T> Empty { get; } = new(new());

    private readonly IndexedSet<T> _source;

    public int Count => _source.Count;
    public T this[int index] => _source[index];

    public ReadOnlyIndexedSet(in IndexedSet<T> source)
    {
        _source = source;
    }

    public bool Contains(in T item)
    {
        return _source.Contains(item);
    }
}

public readonly partial record struct ReadOnlyIndexedSet<T> : IEnumerable<T>
{
    public List<T>.Enumerator GetEnumerator()
    {
        return _source.GetEnumerator();
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
