using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PureClarity.Models;

namespace PureClarity
{
    public abstract class PCCollection<T> where T : PCModelBase
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

        public virtual void AddItem(T item)
        {
            _items.AddOrUpdate(item.Id, item, (key, previousItem) => { return previousItem; });
        }

        public virtual void AddItems(IEnumerable<T> items)
        {
            if (items.Any())
            {
                foreach (var item in items)
                {
                    AddItem(item);
                }
            }
        }

        public virtual void RemoveItemFromCollection(params object[] args)
        {
            var item = (T)Activator.CreateInstance(typeof(T), args);
            _items.TryRemove((string)args[0], out item);
        }

        public virtual void RemoveItemsFromCollection(IEnumerable<string> itemIds)
        {
            if (itemIds.Any())
            {
                foreach (var id in itemIds)
                {
                    RemoveItemFromCollection(id);
                }
            }
        }
    }
}