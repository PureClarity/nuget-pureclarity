# Remove Item Result

Returned when removing an item from a collection. Provides the removed item, indicates the success state and, if failed, the error.

## Properties

Property | Type | Description
------------ | ------------- | ------------- 
Item | T | The item that was removed
Success | bool | Indicates if the remove item call was successful
Error | string | The error provided if the remove item call failed