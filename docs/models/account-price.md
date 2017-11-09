# Account Price

Structured object for representing an account price in PureClarity. Contains a [Price](price) list to apply to an account Id and SKU combination.

## Properties

Property | Type | Description
------------ | ------------- | ------------- 
AccountId | string | The Id of the account 
Sku | string | The SKU of the item that the price will apply to
ParentId | string | The SKU of the parent Product for this Variant. Note that for a [Simple Product](product) this is optional
Prices | List\<Price> | A list of account [Prices](price) for the item
SalePrices | List\<Price> | A list of account sale [Prices](price) for the item
