using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PureClarity;
using PureClarity.Models;
using PureClarity.Validators;

namespace PureClarity.Collections
{
    public class UserCollection : PCCollection<User>
    {
        public override ValidationResult Validate()
        {
            var invalidItems = new Dictionary<string, IEnumerable<string>>();

            foreach (var user in _items)
            {
                if (UserValidator.IsDOBValid(user.Value.DOB))
                {
                    invalidItems.Add(user.Value.Id, new List<string> { "DOB is invalid" });
                }
            }

            return new ValidationResult { Success = invalidItems.Count == 0, InvalidRecords = invalidItems };
        }


    }
}