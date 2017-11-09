# Publish Delta Result

Returned by the PublishDelta (and PublishDeltaAsync) call. Provides the tokens for successfully published deltas and a list of [PublishDeltaError](/publish-delta-error) for those deltas that failed to publish.

## Properties

Property | Type | Description
------------ | ------------- | ------------- 
Tokens | List\<string> | A list of tokens for successfully published deltas
Errors | List\<PublishDeltaError> | A list of [PublishDeltaError](/publish-delta-error) for deltas that failed to publish