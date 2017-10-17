using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PureClarity;
using PureClarity.Models;

namespace PureClarity.Collections
{
    public class UserCollection : PCCollection<User>
    {
        public override ValidatorResult Validate()
        {
            throw new NotImplementedException();
        }
    }
}