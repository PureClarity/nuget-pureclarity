
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using PureClarity.Models;

namespace PureClarity.Models
{
    public class Category : PCModelBase
    {
        [JsonIgnore]
        private readonly string exceptionMessage = "is a mandatory field and must be populated";

        /// <summary>
        /// Unique category id. Must be unique across all categoriues
        /// </summary>
        new public string Id { get => base.Id; set => base.Id = value; }

        /// <summary>
        /// This is the name that will be displayed for each category in recommenders.
        /// </summary>
        public string DisplayName;

        /// <summary>
        /// Optional. An absolute URL pointing to the location of the image. 
        /// This image will be used when displaying category recommenders and will be used in the PureClarity admin
        /// </summary>
        public string Image;

        /// <summary>
        /// A relative or absolute URL for the category listing page on your website.
        /// If using an absolute URL, specify it without the protocol
        /// </summary>
        public string Link;

        /// <summary>
        /// Optional. The Id's of the parent categories for this category. 
        /// This means that a category can exist in more than one parent category
        /// </summary>
        public string[] ParentIds;

        /// <summary>
        /// Optional. Breadcrumb trail to the category listing page. 
        /// If not set, then it is automatically generated from the ParentId's.
        /// </summary>
        public string Breadcrumb;

        /// <summary>
        /// Optional. A short, non-formatted description of the category. 
        /// This field should not contain any HTML
        /// </summary>
        public string Description;


        public Category(string id, string displayName, string link)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException($"{nameof(id)} {exceptionMessage}", nameof(id));
            }

            if (string.IsNullOrEmpty(displayName))
            {
                throw new ArgumentException($"{nameof(displayName)} {exceptionMessage}", nameof(displayName));
            }

            if (string.IsNullOrEmpty(link))
            {
                throw new ArgumentException($"{nameof(link)} {exceptionMessage}", nameof(link));
            }

            Id = id;
            DisplayName = displayName;
            Link = link;
        }

    }
}