# Price

Structured object for representing a price in PureClarity.

## Properties

Property | Type | Description
------------ | ------------- | ------------- 
Currency | string | The [ISO 4217 Code](https://www.iso.org/iso-4217-currency-codes.html) for the currency
Value | decimal | The value in decimal of the price


## Constructor

**`Price(decimal value, string currency)`**

Note that all parameters on the constructor are mandatory. 


## Additional Remarks

When adding prices to a list of prices use multiple prices for different currencies.

When using multiple currencies all items that are required to have prices must have a price entry for each currency that is used in the feed. For example: you cannot have all products with a GBP price and only some products with a USD price, they must all have a GBP price and an USD price.