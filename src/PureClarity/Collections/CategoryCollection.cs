using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PureClarity;
using PureClarity.Models;

namespace PureClarity
{
    public class CategoryCollection : PCCollection<Category>
    {
        public override void AddItem(Category category)
        {
            _items.AddOrUpdate(category.Id, category, (key, previousCategory) => { return category; });
        }

        public override void AddItems(IEnumerable<Category> categories)
        {
            if (categories.Any())
            {
                foreach (var category in categories)
                {
                    AddItem(category);
                }
            }
        }

        public override void RemoveItemFromCollection(string id)
        {
            var category = new Category(id);
            _items.TryRemove(id, out category);
        }

        public override void RemoveItemsFromCollection(IEnumerable<string> categoryIds)
        {
            if (categoryIds.Any())
            {
                foreach (var id in categoryIds)
                {
                    RemoveItemFromCollection(id);
                }
            }

        }
    }
}