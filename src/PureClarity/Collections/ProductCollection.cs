using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PureClarity;
using PureClarity.Models;
using PureClarity.Validators;

namespace PureClarity.Collections
{
    internal class ProductCollection : PCCollection<Product>
    {

        private ConcurrentDictionary<string, List<Product>> _variantsAwaitingParents;

        public ProductCollection()
        {
            _items = new ConcurrentDictionary<string, Product>();
            _variantsAwaitingParents = new ConcurrentDictionary<string, List<Product>>();
        }

        public override AddItemResult AddItem(Product item)
        {
            var result = new AddItemResult
            {
                Success = true
            };

            if (!string.IsNullOrWhiteSpace(item.ParentId))
            {
                if (_items.ContainsKey(item.ParentId))
                {
                    var parentProduct = _items[item.ParentId];

                    if (parentProduct.Variants.All((var) => { return var.Id != item.Id; }))
                    {
                        parentProduct.Variants.Add(item);
                        _items.AddOrUpdate(parentProduct.Id, parentProduct, (key, previousItem) =>
                        {
                            return parentProduct;
                        });
                    }
                    else
                    {
                        result.Success = false;
                        result.Error = $"Duplicate item found: {item.Id}. Newest item not added.";
                    }
                }
                else if (_variantsAwaitingParents.ContainsKey(item.ParentId))
                {
                    var existingList = _variantsAwaitingParents[item.ParentId];

                    if (existingList.All((var) => { return var.Id != item.Id; }))
                    {
                        existingList.Add(item);
                        _variantsAwaitingParents.AddOrUpdate(item.ParentId, existingList, (key, previousItem) =>
                        {
                            return existingList;
                        });
                    }
                    else
                    {
                        result.Success = false;
                        result.Error = $"Duplicate item found: {item.Id}. Newest item not added.";
                    }
                }
                else
                {
                    _variantsAwaitingParents.AddOrUpdate(item.Id, new List<Product> { item }, (key, previousItem) =>
                       {
                           result.Success = false;
                           result.Error = $"Duplicate item found: {item.Id}. Newest item not added.";
                           return previousItem;
                       });
                }

            }
            else
            {
                if (_variantsAwaitingParents.ContainsKey(item.Id))
                {
                    item.Variants.AddRange(_variantsAwaitingParents[item.Id]);
                    var removedVariants = new List<Product>();
                    _variantsAwaitingParents.TryRemove(item.Id, out removedVariants);
                }

                _items.AddOrUpdate(item.Id, item, (key, previousItem) =>
                {
                    result.Success = false;
                    result.Error = $"Duplicate item found: {item.Id}. Newest item not added.";
                    return previousItem;
                });
            }

            return result;
        }

        public override ValidationResult Validate()
        {
            var products = this._items.AsEnumerable().Select((productKVP) =>
            {
                return productKVP.Value;
            });

            var validator = new ProductValidator();

            validator.ValidateUnnassignedVariants(_variantsAwaitingParents);

            validator.GetAllCurrencies(products);

            foreach (var prod in products)
            {
                validator.ValidateProduct(prod);
            }

            return validator.GetValidationResult();
        }
    }
}