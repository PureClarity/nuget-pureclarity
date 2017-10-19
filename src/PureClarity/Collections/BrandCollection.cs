using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PureClarity;
using PureClarity.Models;

namespace PureClarity.Collections
{
    public class BrandCollection : PCCollection<Brand>
    {
        public override ValidatorResult Validate()
        {
            throw new NotImplementedException();
        }
    }
}