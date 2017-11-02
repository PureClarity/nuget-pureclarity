using System;
using System.Collections.Generic;
using System.Text;

namespace PureClarity.Models
{
    public class CollectionState<T>
    {
        /// <summary>
        /// Number  of items in the internal collection
        /// </summary>
        public int ItemCount;

        public List<T> Items;
    }
}
