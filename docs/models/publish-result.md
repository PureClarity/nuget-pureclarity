# Publish Result

Returned by the Publish (and PublishAsync) call. Only those feeds that have items in the collection are published and return a [PublishFeedResult](publish-feed-result).

## Properties

Property | Type | Description
------------ | ------------- | ------------- 
Success | bool | Indicates if all feeds were successfully published
PublishProductFeedResult | PublishFeedResult | A [PublishFeedResult](publish-feed-result) for the Product Feed
PublishCategoryFeedResult | PublishFeedResult | A [PublishFeedResult](publish-feed-result) for the Category Feed
PublishBrandFeedResult | PublishFeedResult | A [PublishFeedResult](publish-feed-result) for the Brand Feed
PublishUserFeedResult | PublishFeedResult |  A [PublishFeedResult](publish-feed-result) for the User Feed