using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PureClarity.Models
{
    public class ProcessedProduct
    {
        /// <summary>
        /// Unique product sku. Must be unique across all products
        /// </summary>
        public string Sku;

        public string[] AssociatedSkus;

        /// <summary>
        /// An array of category ids
        /// </summary>
        public string[] Categories;

        /// <summary>
        /// Name of the product as it will appear in search results and recommendations
        /// </summary>
        public string Title;

        public string[] AssociatedTitles;

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
        public string[] SearchTags;

        /// <summary>
        /// Optional. Set of accounts where this product should be visible. 
        /// If set, then the product will only be visible for these accounts - if no account information is present then these will be hidden from the user.
        /// </summary>
        public string[] AccountInclusions;

        /// <summary>
        /// Optional. Set of accounts where this product should be hidden. 
        /// If set, then the product will only be hidden for these accounts - if no account information is present or the account is not excluded then these will be visible to the user.
        /// </summary>
        public string[] AccountExclusions;

        /// <summary>
        /// Optional. If present, and set to true then this product will be hidden from all recommenders.
        /// </summary>
        public bool? ExcludeFromRecommenders;

        public string[] Prices;
        public string[] SalePrices;

        /// <summary>
        /// Optional. Custom attributes which by default will be assumed to be facets to be used in the search results. 
        /// Products need to only define the attributes they have.
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JToken> Attributes { get; set; }
    }
}