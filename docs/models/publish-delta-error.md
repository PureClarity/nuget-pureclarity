# Publish Delta Error

Returned as part of a [PublishDeltaResult](/publish-delta-result) when a delta fails to publish. Provides the error that occured and a list of Ids for the objects that were part of the delta.

## Properties

Property | Type | Description
------------ | ------------- | ------------- 
Error | string | The error thrown when trying to publish a delta to the API
Skus | IEnumerable\<string> | A list of the SKUs that were in the delta
DeletedSkus | IEnumerable\<string> | A list of the deleted product SKUs that were in the delta
AccountPrices | IEnumerable\<AccountPriceBase> | A list of [AccountPriceBase](/account-price-base) containing the sku and accountId that were in the delta
DeletedAccountPrices | IEnumerable\<AccountPriceBase> | A list of the deleted [AccountPriceBase](/account-price-base) containing the sku and accountId that were in the delta