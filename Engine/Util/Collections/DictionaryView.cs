using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Engine.Util.Collections;

/// <summary>
/// A read-only view of a <c>Dictionary</c>.
/// </summary>
public readonly partial record struct DictionaryView<TKey, TValue>
    where TKey : notnull
{
    private static readonly Dictionary<TKey, TValue> s_emptyDict = new();

    private readonly Dictionary<TKey, TValue> _dict;

    public int Count => _dict.Count;
    public Dictionary<TKey, TValue>.KeyCollection Keys => _dict.Keys;
    public Dictionary<TKey, TValue>.ValueCollection Values => _dict.Values;
    public TValue this[TKey key] => _dict[key];

    public DictionaryView()
        : this(s_emptyDict) { }

    public DictionaryView(Dictionary<TKey, TValue> dict)
    {
        _dict = dict;
    }

    public bool ContainsKey(TKey key)
    {
        return _dict.ContainsKey(key);
    }

    public bool ContainsValue(TValue value)
    {
        return _dict.ContainsValue(value);
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        return _dict.TryGetValue(key, out value);
    }

    public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
    {
        return _dict.GetEnumerator();
    }
}
