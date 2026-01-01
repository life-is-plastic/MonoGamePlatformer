using System;

namespace Engine.Util;

/// <summary>
/// A read-only view of an <c>IndexedSet</c>.
/// </summary>
public readonly record struct IndexedSetView<T>
    where T : notnull
{
    private static readonly IndexedSet<T> s_empty = new();

    private readonly IndexedSet<T> _indexedSet;

    public int Count => _indexedSet.Count;
    public T this[int index] => _indexedSet[index];

    public IndexedSetView()
        : this(s_empty) { }

    public IndexedSetView(IndexedSet<T> indexedSet)
    {
        _indexedSet = indexedSet;
    }

    public ReadOnlySpan<T> AsSpan()
    {
        return _indexedSet.AsSpan();
    }

    public ReadOnlySpan<T>.Enumerator GetEnumerator()
    {
        return _indexedSet.GetEnumerator();
    }

    public bool Contains(T item)
    {
        return _indexedSet.Contains(item);
    }
}
