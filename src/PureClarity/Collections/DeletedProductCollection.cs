using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PureClarity;
using PureClarity.Models;

namespace PureClarity
{
    public class DeletedProductCollection : PCCollection<string>
    {
        public override void AddItem(string sku)
        {
            _items.AddOrUpdate(sku, sku, (key, existingSku) => { return sku; });
        }

        public override void AddItems(IEnumerable<string> skus)
        {
            if (skus.Any())
            {
                foreach (var sku in skus)
                {
                    AddItem(sku);
                }
            }
        }

        public override void RemoveItemFromCollection(string sku)
        {
            _items.TryRemove(sku, out sku);
        }

        public override void RemoveItemsFromCollection(IEnumerable<string> skus)
        {
            if (skus.Any())
            {
                foreach (var sku in skus)
                {
                    RemoveItemFromCollection(sku);
                }
            }

        }
    }
}