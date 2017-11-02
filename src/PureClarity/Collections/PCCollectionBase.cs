using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PureClarity.Models;

namespace PureClarity.Collections
{
    internal abstract class PCCollection<T> where T : PCModelBase
    {
        protected ConcurrentDictionary<string, T> _items;

        public PCCollection()
        {
            _items = new ConcurrentDictionary<string, T>();
        }

        public virtual AddItemResult AddItem(T item)
        {
            var result = new AddItemResult
            {
                Success = true
            };

            _items.AddOrUpdate(item.Id, item, (key, previousItem) =>
            {
                result.Success = false;
                result.Error = $"Duplicate item found: {item.Id}. Newest item not added.";
                return previousItem;
            });

            return result;
        }

        public virtual IEnumerable<AddItemResult> AddItems(IEnumerable<T> items)
        {
            var results = new List<AddItemResult>();
            if (items.Any())
            {
                foreach (var item in items)
                {
                    results.Add(AddItem(item));
                }
            }
            return results;
        }

        public virtual RemoveItemResult<T> RemoveItemFromCollection(string id)
        {
            var result = new RemoveItemResult<T>();
            T item;
            result.Success = _items.TryRemove(id, out item);
            result.Item = item;
            result.Error = !result.Success ? $"{id} could not be removed." : null;
            return result;
        }

        public virtual IEnumerable<RemoveItemResult<T>> RemoveItemsFromCollection(IEnumerable<string> itemIds)
        {
            var results = new List<RemoveItemResult<T>>();
            if (itemIds.Any())
            {
                foreach (var id in itemIds)
                {
                    results.Add(RemoveItemFromCollection(id));
                }
            }
            return results;
        }

        public virtual IEnumerable<T> GetItems()
        {
            return _items.Select((itemKVP) => itemKVP.Value);
        }

        public abstract ValidationResult Validate();

        /// <summary>
        /// Gets useful information on the internal state of the collection
        /// </summary>
        /// <returns>CollectionState containing useful information</returns>
        public CollectionState<T> GetCollectionState()
        {
            return new CollectionState<T> { ItemCount = _items.Count, Items = this._items.Values.ToList() };
        }
    }
}