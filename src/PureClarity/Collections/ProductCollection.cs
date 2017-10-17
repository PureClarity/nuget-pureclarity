using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PureClarity;
using PureClarity.Models;
using PureClarity.Validators;

namespace PureClarity.Collections
{
    public class ProductCollection : PCCollection<Product>
    {
        public override ValidatorResult Validate()
        {
            var products = this._items.AsEnumerable().Select((productKVP) =>
            {
                return productKVP.Value;
            });

            var validator = new ProductValidator();

            validator.GetAllCurrencies(products);

            foreach (var prod in products)
            {
                validator.ValidateProduct(prod);
            }

            return validator.GetValidationResult();
        }
    }
}