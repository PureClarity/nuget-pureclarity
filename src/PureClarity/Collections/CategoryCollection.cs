using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PureClarity;
using PureClarity.Models;

namespace PureClarity.Collections
{
    public class CategoryCollection : PCCollection<Category>
    {
        public override ValidatorResult Validate()
        {
            return new ValidatorResult { Success = true };
        }
    }
}