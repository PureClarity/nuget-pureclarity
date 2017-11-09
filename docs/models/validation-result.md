# Validation Result

Returned when validating the feed. Indicates if the validation was a success and provides a [CollectionValidationResult](collection-validation-result) for each collection.

## Properties

Property | Type | Description
------------ | ------------- | ------------- 
Success | bool | Indicates if all collections validated successfully
ProductValidationResult | CollectionValidationResult | A [CollectionValidationResult](collection-validation-result) for the Product Collection
AccountPriceValidationResult | CollectionValidationResult | A [CollectionValidationResult](collection-validation-result) for the Account Price Collection
CategoryValidationResult | CollectionValidationResult | A [CollectionValidationResult](collection-validation-result) for the Category Collection
BrandValidationResult | CollectionValidationResult | A [CollectionValidationResult](collection-validation-result) for the Brand Collection
UserValidationResult | CollectionValidationResult | A [CollectionValidationResult](collection-validation-result) for the User Collection
