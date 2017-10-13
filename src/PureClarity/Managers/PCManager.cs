using System.Collections.Concurrent;
using System.Collections.Generic;
using PureClarity.Models;

namespace PureClarity
{
    public abstract class PCManager<T>
    {
        protected ConcurrentDictionary<string, T> _items;

        public PCManager()
        {
            _items = new ConcurrentDictionary<string, T>();
        }

        /// <summary>
        /// Gets useful information on the internal state of the manager
        /// </summary>
        /// <returns>ManagerState containing useful information</returns>
        public ManagerState GetManagerState()
        {
            return new ManagerState { ItemCount = _items.Count };
        }

        public abstract void AddItem(T item);
        public abstract void AddItems(IEnumerable<T> items);

        public abstract void RemoveItem(string itemId);
        public abstract void RemoveItems(IEnumerable<string> itemIds);
    }
}