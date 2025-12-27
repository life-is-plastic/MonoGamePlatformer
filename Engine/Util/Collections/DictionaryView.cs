using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Engine.Util.Collections;

/// <summary>
/// A read-only view of a <c>Dictionary</c>.
/// </summary>
public readonly partial record struct DictionaryView<TKey, TValue>
    where TKey : notnull
{
    public static DictionaryView<TKey, TValue> Empty { get; } = new(new());

    private readonly Dictionary<TKey, TValue> _dict;

    public int Count => _dict.Count;
    public Dictionary<TKey, TValue>.KeyCollection Keys => _dict.Keys;
    public Dictionary<TKey, TValue>.ValueCollection Values => _dict.Values;
    public TValue this[TKey key] => _dict[key];

    public DictionaryView(Dictionary<TKey, TValue> dict)
    {
        _dict = dict;
    }

    public bool ContainsKey(in TKey key)
    {
        return _dict.ContainsKey(key);
    }

    public bool ContainsValue(in TValue value)
    {
        return _dict.ContainsValue(value);
    }

    public bool TryGetValue(in TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        return _dict.TryGetValue(key, out value);
    }
}

public readonly partial record struct DictionaryView<TKey, TValue>
    : IEnumerable<KeyValuePair<TKey, TValue>>
{
    public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
    {
        return _dict.GetEnumerator();
    }

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
    {
        return GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
