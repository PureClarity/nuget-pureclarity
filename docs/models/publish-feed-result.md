# Publish Feed Result

Returned as part of a [PublishResult](publish-result) when publishing feeds. Provides success state and, if it failed, an error along with a stacktrace.

## Properties

Property | Type | Description
------------ | ------------- | ------------- 
Success | bool | Indicates if the feed was successfully published
StackTrace | string | Stacktrace produced by the error
Error | string | Error thrown when trying to publish feed