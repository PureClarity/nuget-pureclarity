# Collection State

Provides a readonly view of the internal collection.

## Properties

Property | Type | Description
------------ | ------------- | ------------- 
ItemCount | int | The number of items in the collection
Items | IReadOnlyCollection<T> | A readonly view of the items in the collection

## Remarks

Note that the ItemCount is not the same as the number of items added to the collection but the count of the number of top level items in a collection. Collections may perform additional logic upon adding an item which results in the item been added as a subitem in certain scenarios.

An example of this is the AddProduct call in the [FeedManager](/nuget-pureclarity/managers/feed-manager). If a Variant is added it will be assigned to the Variants list property on the Product specified by the Variants ParentId. This means that, although the item was successfully added, the ItemCount remains the same.