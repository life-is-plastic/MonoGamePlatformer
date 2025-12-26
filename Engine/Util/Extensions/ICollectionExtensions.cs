using System.Collections.Generic;
using System.Diagnostics;

namespace Engine.Util.Extensions;

public static class ICollectionExtensions
{
    extension<TCollection, TItem>(TCollection collection)
        where TCollection : ICollection<TItem>
    {
        /// <summary>
        /// Throws an exception if the item already exists.
        /// </summary>
        public void AddOrDie(TItem item)
        {
            Debug.Assert(!collection.Contains(item));
            collection.Add(item);
        }

        /// <summary>
        /// Throws an exception if the item was not found.
        /// </summary>
        public void RemoveOrDie(TItem item)
        {
            Debug.Assert(collection.Contains(item));
            collection.Remove(item);
        }
    }
}
