# Query Token Manager

Information on querying tokens.

## Initialisation

**`new QueryTokenManager(string accessKey, int region)`**

Sets the Access Key and Region to use when making calls to PureClarity. These details will be provided to you at signup. 

The Access Key identifies the client.
The Region is used to select the appropriate endpoints to use.

## Querying Tokens

### Query Tokens

**`QueryTokensResult QueryTokens(IEnumerable<string> tokens)`**

**`Task<QueryTokensResult> QueryTokensAsync(IEnumerable<string> tokens)`**

Queries the status of the product deltas based on the tokens returned when they were published. An async version is provided and is the preferred usage.

Deltas can be in the following states:

* 0: Pending  - 	The delta has not been processed yet.
* 1: Success  -	 The delta was applied successfully.
* 2: Error	   -  The delta failed. 

If a delta failed then the reason will also be provided.