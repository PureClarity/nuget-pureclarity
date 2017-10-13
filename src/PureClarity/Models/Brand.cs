
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PureClarity
{
    public class Brand
    {
        /// <summary>
        /// Unique brand id. Must be unique across all brands
        /// </summary>
        public string Id;

        /// <summary>
        /// This is the name that will be displayed for each brand in recommenders.
        /// </summary>
        public string DisplayName;

        /// <summary>
        /// Optional. An absolute URL pointing to the location of the image. 
        /// This image will be used when displaying brand recommenders
        /// </summary>
        public string Image;      

        /// <summary>
        /// Optional. A short, non-formatted description of the category. 
        /// This field should not contain any HTML
        /// </summary>
        public string Description;        


        public Brand(string id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }        
        
    }
}