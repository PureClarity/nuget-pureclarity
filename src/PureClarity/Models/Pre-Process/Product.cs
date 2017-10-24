
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using PureClarity.Models;

namespace PureClarity.Models
{
    public class Product : PCModelBase
    {
        /// <summary>
        /// Unique product sku. Must be unique across all products
        /// </summary>
        public string Sku { get => Id; set => Id = value; }

        /// <summary>
        /// An array of category ids
        /// </summary>
        public List<string> Categories;

        /// <summary>
        /// Name of the product as it will appear in search results and recommendations
        /// </summary>
        public string Title;

        /// <summary>
        /// A short non-formatted description of the product. This should not contain HTML
        /// </summary>
        public string Description;

        /// <summary>
        /// A relative or absolute URL for the products page on your website.
        /// If using an absolute URL, specify it without the protocol
        /// </summary>
        public string Link;

        /// <summary>
        /// An absolute URL pointing to the image to display for the item in your search results.
        /// </summary>
        public string Image;

        /// <summary>
        /// Optional. A relative or absolute URL pointing to an overlay image to display for the item in your search results.
        /// </summary>
        public string ImageOverlay;

        /// <summary>
        /// Optional. Brand identifier
        /// </summary>
        public string Brand;

        /// <summary>
        /// Optional. Used by some of the Behavioral Merchandising recommenders to identify sale items.
        /// </summary>
        public bool? OnOffer;

        /// <summary>
        /// Used by some of the Behavioral Merchandising recommenders to identify new items.
        /// </summary>
        public bool? NewArrival;

        /// <summary>
        /// Used to group variants together. If set then the record is assumed to be a variant, with a parent product identified by ParentId.
        /// </summary>
        public string ParentId;

        /// <summary>
        /// Optional. Controls whether to use the products "default" price when no account price is found for an account. Default (if not present) is false
        /// </summary>
        public bool? NoDefaultPriceForAccounts;

        /// <summary>
        /// Optional. Array of additional text to use when searching for products. 
        /// </summary>
        public List<string> SearchTags;

        /// <summary>
        /// Optional. Set of accounts where this product should be visible. 
        /// If set, then the product will only be visible for these accounts - if no account information is present then these will be hidden from the user.
        /// </summary>
        public List<string> AccountInclusions;

        /// <summary>
        /// Optional. Set of accounts where this product should be hidden. 
        /// If set, then the product will only be hidden for these accounts - if no account information is present or the account is not excluded then these will be visible to the user.
        /// </summary>
        public List<string> AccountExclusions;

        /// <summary>
        /// Optional. If present, and set to true then this product will be hidden from all recommenders.
        /// </summary>
        public bool? ExcludeFromRecommenders;

        /// <summary>
        /// Optional. Product variants
        /// </summary>
        public List<Product> Variants;

        public List<Price> Prices;
        public List<Price> SalePrices;

        /// <summary>
        /// Optional. Custom attributes which by default will be assumed to be facets to be used in the search results. 
        /// Products need to only define the attributes they have.
        /// </summary>
        public IDictionary<string, IEnumerable<string>> Attributes { get; set; }

        const string MandatoryFieldMessage = "is a mandatory field";

        public Product(string sku, string title, string description, string link, string image, List<string> categories)
        {
            Sku = sku ?? throw new ArgumentNullException(nameof(sku), $"{nameof(sku)} {MandatoryFieldMessage}");
            Title = title ?? throw new ArgumentNullException(nameof(title), $"{nameof(title)} {MandatoryFieldMessage}");
            Description = description ?? throw new ArgumentNullException(nameof(description), $"{nameof(description)} {MandatoryFieldMessage}");
            Link = link ?? throw new ArgumentNullException(nameof(link), $"{nameof(link)} {MandatoryFieldMessage}");
            Image = image ?? throw new ArgumentNullException(nameof(image), $"{nameof(image)} {MandatoryFieldMessage}");
            Categories = categories ?? throw new ArgumentNullException(nameof(categories), $"{nameof(categories)} {MandatoryFieldMessage}");
            Prices = new List<Price>();
            SalePrices = new List<Price>();
            Variants = new List<Product>();
            Attributes = new Dictionary<string, IEnumerable<string>>();
        }

    }
}