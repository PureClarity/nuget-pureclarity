using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PureClarity;
using PureClarity.Models;

namespace PureClarity.Collections
{
    public class DeletedAccountPriceCollection : PCCollection<DeletedAccountPrice>
    {
        public override ValidationResult Validate()
        {
            throw new NotImplementedException();
        }
    }
}