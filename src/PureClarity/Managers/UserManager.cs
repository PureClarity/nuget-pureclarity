using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PureClarity;
using PureClarity.Models;

namespace PureClarity
{
    public class UserManager : PCManager<User>
    {
        public override void AddItem(User user)
        {
            _items.AddOrUpdate(user.UserId, user, (key, previousBrand) => { return user; });
        }

        public override void AddItems(IEnumerable<User> users)
        {
            if (users.Any())
            {
                foreach (var user in users)
                {
                    AddItem(user);
                }
            }
        }

        public override void RemoveItem(string id)
        {
            var user = new User(id);
            _items.TryRemove(id, out user);
        }

        public override void RemoveItems(IEnumerable<string> userIds)
        {
            if (userIds.Any())
            {
                foreach (var id in userIds)
                {
                    RemoveItem(id);
                }
            }

        }
    }
}