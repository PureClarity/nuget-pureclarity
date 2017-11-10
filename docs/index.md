# PureClarity SDK Documentation

## Managers

Classes containing the methods you will use to interact with the SDK.

### [Feeds](managers/feed-manager)

Creating, validating and publishing feeds.


### [Tokens](managers/query-token-manager)

Querying tokens.

## Models

The objects you will be dealing with when using the PureClarity SDK. 

These are grouped into two sections:

* **Input:** Objects you populate and then pass in to the SDK
* **Output:** Objects that are returned by the SDK to indicate action success and state

### Input

* [Product](models/product)
* [AccountPrice](models/account-price)
* [DeletedAccountPrice](models/deleted-account-price)
* [Category](models/category)
* [User](models/user)
* [Brand](models/brand)

### Output

* [AddItemResult](models/add-item-result)
* [RemoveItemResult](models/remove-item-result)
* [CollectionState](models/collection-state)
* [ValidationResult](models/validation-result)
* [PublishResult](models/publish-result)
* [PublishDeltaResult](models/publish-delta-result)