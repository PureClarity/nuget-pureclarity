# Collection Validation Result

Returned as part of a [ValidationResult](validation-result) when validating the feed. Provides success state and, if it failed, a list of invalid records.

## Properties

Property | Type | Description
------------ | ------------- | ------------- 
Success | bool | Indicates if all collections validated successfully
InvalidRecords | IDictionary\<string, IEnumerable\<string>> | A list of validation errors for each record id.
