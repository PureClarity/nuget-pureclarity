using System.Collections.Concurrent;
using System.Collections.Generic;
using PureClarity.Models;

namespace PureClarity
{
    public abstract class PCCollection<T>
    {
        protected ConcurrentDictionary<string, T> _items;

        public PCCollection()
        {
            _items = new ConcurrentDictionary<string, T>();
        }

        /// <summary>
        /// Gets useful information on the internal state of the manager
        /// </summary>
        /// <returns>CollectionState containing useful information</returns>
        public CollectionState GetCollectionState()
        {
            return new CollectionState { ItemCount = _items.Count };
        }

        public abstract void AddItem(T item);
        public abstract void AddItems(IEnumerable<T> items);

        public abstract void RemoveItemFromCollection(string itemId);
        public abstract void RemoveItemsFromCollection(IEnumerable<string> itemIds);
    }
}