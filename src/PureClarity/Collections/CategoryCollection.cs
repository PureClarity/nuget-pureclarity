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
        public override ValidationResult Validate()
        {
            return new ValidationResult { Success = true };
        }
    }
}