using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PureClarity;
using PureClarity.Models;

namespace PureClarity
{
    public class ProductManager : PCManager<Product>
    {
        public override void AddItem(Product product)
        {
            _items.AddOrUpdate(product.Sku, product, (key, previousProduct) => { return product; });
        }

        public override void AddItems(IEnumerable<Product> products)
        {
            if (products.Any())
            {
                foreach (var product in products)
                {
                    AddItem(product);
                }
            }
        }

        public override void RemoveItem(string sku)
        {
            var prod = new Product(sku);
            _items.TryRemove(sku, out prod);
        }

        public override void RemoveItems(IEnumerable<string> skus)
        {
            if (skus.Any())
            {
                foreach (var sku in skus)
                {
                    RemoveItem(sku);
                }
            }

        }
    }
}