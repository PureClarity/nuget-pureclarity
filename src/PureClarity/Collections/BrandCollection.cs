using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PureClarity;
using PureClarity.Models;

namespace PureClarity.Collections
{
    internal class BrandCollection : PCCollection<Brand>
    {
        public override ValidationResult Validate()
        {
            throw new NotImplementedException();
        }
    }
}