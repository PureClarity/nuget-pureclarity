using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PureClarity;
using PureClarity.Models;

namespace PureClarity.Collections
{
    internal class DeletedAccountPriceCollection : PCCollection<DeletedAccountPrice>
    {
        public override CollectionValidationResult Validate()
        {
            throw new NotImplementedException();
        }
    }
}