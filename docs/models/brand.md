# Brand

Structured object for representing a brand in PureClarity.

## Properties

Property | Type | Description | Mandatory
------------ | ------------- | ------------- | ------------- 
Id | string | The unique Id of the brand | Yes
DisplayName | string | This is the name that will be displayed for each brand in recommenders. | Yes
Image | string | An absolute URL pointing to the location of the image. This image will be used when displaying brand recommenders or brand results in an autocomplete and will be used in the PureClarity admin. If possible, specify the URL without a protocol e.g. // instead of http:// or https:// | No
Description | string |  A short, non-formatted description of the brand. This field should not contain any HTML | No


## Constructor

**`Brand(string id)`**

Note that all parameters on the constructor are mandatory. 


## Remarks

Note that there is no Link URL as when a brand is selected it will automatically redirect to the search results page. 

Description can be used in merchandising recommenders that show brands (if required).
