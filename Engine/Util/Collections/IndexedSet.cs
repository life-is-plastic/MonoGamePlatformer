using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Engine.Util.Collections;

/// <summary>
/// A set where insertion, deletion, lookup by item, and lookup by index all perform in O(1). Items
/// are stored contiguously in memory to enable fast iteration. Insertions always append to the end
/// of the set. Deletions do not preserve order.
/// </summary>
public readonly record struct IndexedSet<T>
    where T : notnull
{
    private readonly Dictionary<T, int> _indices;
    private readonly List<T> _items;

    public int Count => _items.Count;
    public T this[int index] => _items[index];

    public IndexedSet()
        : this(0) { }

    public IndexedSet(int capacity)
    {
        _indices = new(capacity);
        _items = new(capacity);
    }

    public Span<T> AsSpan()
    {
        return CollectionsMarshal.AsSpan(_items);
    }

    public ReadOnlySpan<T>.Enumerator GetEnumerator()
    {
        ReadOnlySpan<T> span = AsSpan();
        return span.GetEnumerator();
    }

    public bool Contains(T item)
    {
        return _indices.ContainsKey(item);
    }

    /// <summary>
    /// Returns true if the item was successfully added, and false if the item already existed
    /// beforehand.
    /// </summary>
    public bool Add(T item)
    {
        if (_indices.TryAdd(item, _items.Count))
        {
            _items.Add(item);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Throws an exception if the item already exists.
    /// </summary>
    public void AddOrDie(T item)
    {
        var added = Add(item);
        Debug.Assert(added);
    }

    /// <summary>
    /// Returns true if the item was successfully removed, and false if the item was not found.
    /// </summary>
    public bool Remove(T item)
    {
        if (!_indices.TryGetValue(item, out var index))
        {
            return false;
        }
        Swap(index, _items.Count - 1);
        _indices.Remove(item);
        _items.RemoveAt(_items.Count - 1);
        return true;
    }

    /// <summary>
    /// Throws an exception if the item was not found.
    /// </summary>
    public void RemoveOrDie(T item)
    {
        var removed = Remove(item);
        Debug.Assert(removed);
    }

    public void Clear()
    {
        _indices.Clear();
        _items.Clear();
    }

    public void Sort(Comparison<T> comparison)
    {
        _items.Sort(comparison);
        RebuildIndices();
    }

    private void Swap(int i, int j)
    {
        (_indices[_items[i]], _indices[_items[j]]) = (j, i);
        (_items[i], _items[j]) = (_items[j], _items[i]);
    }

    private void RebuildIndices()
    {
        for (var i = 0; i < _items.Count; i++)
        {
            _indices[_items[i]] = i;
        }
    }
}
