# Product

Object structure and remarks.

## Naming Remarks

Throughout this documentation you will see references to both Product and Variant. For the purposes of this SDK a Product is the parent entity and a Variant is the child entity. So a Product will have one or more Variants. They are both represented by the Product object in code.

Where the documentation refers to a Simple Product this is a mapping of single Product to a single Variant where both share the same Sku, or where only the Product exists. In this case no Variant is needed, all required properties can be assigned to the Product alone.

## Properties

Property | Type | Description | Mandatory
------------ | ------------- | ------------- | ------------- 
Sku | string | The unique identifier for a product (or variant). SKU's are required to be unique across all records. | Yes
ParentId | string | Id of the parent Product. This property is used to group variants together. If set then the item is assumed to be a Variant. | No
Title | string | The name of the item as it will appear in search results and recommenders | Yes
Description | string | A short non-formatted description of the item. This should not contain HTML | Yes
Categories | List\<string> | A list of category Ids. This should contain the id of all categories the item is in. | Yes
Link | string | A relative or absolute URL pointing to the item's detail page on your website. If using an absolute URL, specify the URL without a protocol e.g. // instead of http:// or https;// | Yes
Image | string | An absolute URL pointing to the image to display for the item in your search results. It is best to reference an image which has the suitable dimensions to display in the search. If possible, specify the URL without a protocol e.g. // instead of http:// or https;// | Yes
ImageOverlay | string | A relative or absolute URL pointing to an overlay image to display for the item in your search results. It is best to reference an image which has the suitable dimensions to display in the search. If possible, specify the URL without a protocol e.g. // instead of http:// or https;// | No
Brand | string | Optional brand identifier. Used to show brand information in the search auto-complete, and can be used in the templates. | No
Attributes | Dictionary\<string, List\<string>> | Attributes can be consumed by the feed, and by default will be assumed to be facets to be used in the search results. Attributes consist of key value pairs where the key is the attribute name and the value is a list of attribute values for the item. If an item doesn't define an attribute then do not include it in the JSON. The text to show for each facet in the search results will by default be based on the attribute name. CamelCasedAttributes will be split by capital letter, (i.e. be displayed on the search results page as, “Camel Cased Attributes”). Underscore characters, '_', can also be used as word delimiters (i.e. the attribute, “shirt_color” will be shown as “Shirt Color”). | No
SearchTags | List\<string> | Optional array of text to use when searching for items. You can include multiple search tags. Using SearchTags will improve the performance of PureClarity. | No
AccountInclusions | List\<string> | Set of accounts where this product should be visible. If set, then the product will only be visible for these accounts - if no account information is present then these will be hidden from the user. This attribute is only valid on a record where no ParentId is set and is only applicable for B2B sites, where account information is being sent. It is mutually exclusive to AccountExclusions and a record with both is invalid. | No
AccountExclusions | List\<string> | Set of accounts where this product should be hidden. If set, then the product will only be hidden for these accounts - if no account information is present or the account is not excluded then these will be visible to the user. This attribute is only valid on a record where no ParentId is set and is only applicable for B2B sites, where account information is being sent. It is mutually exclusive to AccountInclusions and a record with both is invalid. | No
ExcludeFromRecommenders | bool | If present, and set to true then this product will be hidden from all recommenders. | No
Prices | List\<Price> | A list of prices for the item. This should only be set on Variants and Simple Products. | Y (for variants and simple products)
SalePrices | List\<Price> | A list of sale prices for the item. This should only be set on Variants and Simple Products. | N
NoDefaultPriceForAccounts | bool | Controls whether to use the products "default" price when no account price is found for an account. Default (if not present) is "false", meaning that if the site passes through an account id that doesn't have a price defined for this product, then the price of the product defined in the product and variants will be used. If it is true, then if the site passes through an account id that doesn't have a price defined for this product, then the product will be hidden from the user. | No
OnOffer | bool | Used by some of the Behavioral Merchandising recommenders to identify sale items. | No
NewArrival | bool | Used by some of the Behavioral Merchandising recommenders to identify new items. | No
Variants | List\<Product> | A list of variants belonging to the Product if this is known at the point of adding a Product. This is optional and should only be applied to a Product. Variants can be added to the feeds product collection at any point and will automatically be assigned to a Product based on the ParentId of the Variant. | No

## Constructor

**`Product(string sku, string title, string description, string link, string image, List<string> categories)`**

Note that all parameters on the constructor are mandatory. 

If the product has no categories then an empty list should be passed.


## Additional Remarks

Below are some points to keep in mind whilst using the SDK:

**JSON format:** SKU's must be unique across all the records in the product feed.

Variant records cannot have the same SKU as parent records. If your product data is structured in this way, you will need to alter the SKU's you send to ensure they are all unique.

**JSON format:** Variants must have ParentId set – Parent records must not have this field set.

The ParentId record is used to group related variants together under a single parent record. Ensure this is set. For parent records – this must NOT be set. It is invalid to set this to an empty string for parent records.

**Prices are mandatory for variants and simple products that have no variants.**

Do not set the Price or SalePrices fields for parent records.

**Only set fields that are present on the products.**

Some fields will only be present on some records. For other products, do not set these fields to an empty string – do not set them at all.

**SearchTags should be an array of all the extra pieces of information that PureClarity should use when performing a search.**

Note that by default PureClarity will use the title, SKU and description fields to perform searches on.

**Image URL is required to be an absolute URL.**