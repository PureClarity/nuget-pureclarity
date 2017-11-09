using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PureClarity;
using PureClarity.Models;

namespace PureClarity.Collections
{
    internal class CategoryCollection : PCCollection<Category>
    {
        public override CollectionValidationResult Validate()
        {
            return new CollectionValidationResult { Success = true };
        }
    }
}