using System.Collections.Generic;
using System.Diagnostics;

namespace Engine.Util.Extensions;

public static class IDictionaryExtensions
{
    extension<TKey, TValue>(IDictionary<TKey, TValue> dict)
        where TKey : notnull
    {
        /// <summary>
        /// Throws an exception if the key already exists.
        /// </summary>
        public void AddOrDie(TKey key, TValue value)
        {
            var added = dict.TryAdd(key, value);
            Debug.Assert(added);
        }

        /// <summary>
        /// Throws an exception if the key was not found.
        /// </summary>
        public void RemoveOrDie(TKey key)
        {
            var removed = dict.Remove(key);
            Debug.Assert(removed);
        }
    }

    extension<TKey, TValue>(IDictionary<TKey, TValue> dict)
        where TKey : notnull
        where TValue : new()
    {
        /// <summary>
        /// Gets the value associated with the given key. If the key does not exist, inserts and returns
        /// a new value corresponding to that key.
        /// </summary>
        public TValue GetOrAddNew(TKey key)
        {
            if (dict.TryGetValue(key, out var value))
            {
                return value;
            }
            value = new();
            dict[key] = value;
            return value;
        }
    }
}
