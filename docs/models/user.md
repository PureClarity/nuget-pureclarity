# User

Structured object for representing a user in PureClarity.

## Properties

Property | Type | Description | Mandatory
------------ | ------------- | ------------- | ------------- 
UserId | string | The unique Id of the user | Yes
Email | string | The users email address | No
FirstName | string | The users first name | No
LastName | string | The users last name | No
Salutation | string | Mr, Mrs, Miss, Ms, Dr, etc.. | No
DOB | string | Date of Birth Note: PureClarity derives 'Age' from DOB for Behavioral Profiling. Format is "DD/MM/YYYY" | No
Gender | string | The users gender | No
City | string | Location of City or Town | No
State | string | US State [USPS 2 letter code for US States](https://pe.usps.com/text/pub28/28apb.htm) | No
Country | string | Country location | No
CustomFields | IDictionary\<string, List\<string>> | A custom field, e.g. Marketing Preference, favourite colour etc. Custom Fields consist of key value pairs, key : custom field name, value: list of custom field values. | No

## Constructor

**`User(string userId)`**

Note that all parameters on the constructor are mandatory. 


