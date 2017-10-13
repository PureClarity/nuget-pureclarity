using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PureClarity;
using PureClarity.Models;

namespace PureClarity
{
    public class BrandCollection : PCCollection<Brand>
    {
        public override void AddItem(Brand brand)
        {
            _items.AddOrUpdate(brand.Id, brand, (key, previousBrand) => { return brand; });
        }

        public override void AddItems(IEnumerable<Brand> brands)
        {
            if (brands.Any())
            {
                foreach (var brand in brands)
                {
                    AddItem(brand);
                }
            }
        }

        public override void RemoveItemFromCollection(string id)
        {
            var brand = new Brand(id);
            _items.TryRemove(id, out brand);
        }

        public override void RemoveItemsFromCollection(IEnumerable<string> brandIds)
        {
            if (brandIds.Any())
            {
                foreach (var id in brandIds)
                {
                    RemoveItemFromCollection(id);
                }
            }

        }
    }
}