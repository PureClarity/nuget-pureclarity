# Feed Manager

Information on creating, validating and publishing feeds.

## Initialisation

**`new FeedManager(string accessKey, string secretKey, int region)`**

Sets the Access Key, Secret Key and Region to use when making calls to PureClarity. These details will be provided to you at signup. 

The Access Key identifies the client.
The Secret Key is used for authentication when publishing a feed. It should be treated like a password and kept secure at all times.
The Region is used to select the appropriate endpoints to use.


## Adding and Removing Items


### Add Product(s)
 
**`AddItemResult AddProduct(Product product)`**

**`IEnumerable<AddItemResult> AddProducts(IEnumerable<Product> products)`**

Adds a product (or products) that you wish to send to PureClarity in a feed to an internal collection. If a product with the same Sku has already been added then the AddItemResult will return with Success: false and an error. 

There are no limitations on the order for adding variants  for products:

* Items with a ParentId will be treated as a variant and added to the parent product Variant list. 
* If a variant is added before its parent product then it will be added to the parent product when the parent product is added to the collection.
* Variants can be added to a parent product object before it is added to a collection.

Note that when you call the validation step it will return a status of false if a variant is added and no matching parent product is added before validation. An invalid record message will be provided stating the variant Id and the missing product Id.


### Remove Product(s)

**`RemoveItemResult<Product> RemoveProduct(string productSku)`**

**`IEnumerable<RemoveItemResult<Product>> RemoveProducts(IEnumerable<string> productSkus)`**

Removes a product (or products) by Sku from the internal collection. If Sku is not in the internal collection then this does nothing.


### Add Deleted Product Sku(s)
 
**`AddItemResult AddDeletedProductSku(string productSku)`**

**`IEnumerable<AddItemResult> AddDeletedProductSkus(IEnumerable<string> productSkus)`**

Adds a deleted product (or deleted products) sku that you wish to send to PureClarity in a feed to an internal collection. If a product with the same Sku has already been added then the AddItemResult will return with Success: false and an error. 

This is used to send the sku of a product which you no longer want PureClarity to show. It is only required for a delta. A full feed does not require this.


### Remove Deleted Product Sku(s)

**`RemoveItemResult<DeletedProductSku> RemoveDeletedProductSku(string productSku)`**

**`IEnumerable<RemoveItemResult<DeletedProductSku>> RemoveDeletedProductSkus(IEnumerable<string> productSkus)`**

Removes a deleted product (or deleted products) by Sku from the internal collection. If Sku is not in the internal collection then this does nothing.


### Add Account Price(s)
 
**`AddItemResult AddAccountPrice(AccountPrice accountPrice)`**

**`IEnumerable<AddItemResult> AddAccountPrices(IEnumerable<AccountPrice> accountPrices)`**

Adds an account price (or account prices) that you wish to send to PureClarity in a feed to an internal collection. If an account price with the same combination of Account Id and Sku has already been added then the AddItemResult will return with Success: false and an error. 


### Remove Account Price(s)

**`RemoveItemResult<AccountPrice> RemoveAccountPrice(string accountPriceId)`**

**`RemoveItemResult<AccountPrice> RemoveAccountPrice(string accountId, string sku)`**

**`IEnumerable<RemoveItemResult<AccountPrice>> RemoveAccountPrices(IEnumerable<string> accountPriceIds)`**

Removes an account price by account price Id or by account Id and sku from the internal collection. If account price Id is not in the internal collection then this does nothing.

Account price id is a string in the format of "\<accountId>|\<sku>"


### Add Deleted Account Price(s)
 
**`AddItemResult AddDeletedAccountPrice(DeletedAccountPrice deletedAccountPrice)`**

**`IEnumerable<AddItemResult> AddDeletedAccountPrices(IEnumerable<DeletedAccountPrice> accountPrices)`**

Adds a deleted account price (or deleted account prices) that you wish to send to PureClarity in a feed to an internal collection. If a deleted account price with the same combination of Account Id and Sku has already been added then the AddItemResult will return with Success: false and an error. 


### Remove Deleted Account Price(s)

**`RemoveItemResult<DeletedAccountPrice> RemoveDeletedAccountPrice(string deletedAccountPriceId)`**

**`RemoveItemResult<DeletedAccountPrice> RemoveAccountPrice(string accountId, string sku)`**

**`IEnumerable<RemoveItemResult<DeletedAccountPrice>> RemoveDeletedAccountPrices(IEnumerable<string> deletedAccountPriceIds)`**

Removes a deleted account price by account price Id or by account Id and sku from the internal collection. If account price Id is not in the internal collection then this does nothing.

This is used to send the account id and sku for an account price which you no longer want PureClarity to show. It is only required for a delta. A full feed does not require this.

Account price id is a string in the format of "\<accountId>|\<sku>"


### Add Category(ies)
 
**`AddItemResult AddCategory(Category category)`**

**`IEnumerable<AddItemResult> AddCategories(IEnumerable<Category> categories)`**

Adds a Category (or Categories) that you wish to send to PureClarity in a feed to an internal collection. If a Category with the same Id has already been added then the AddItemResult will return with Success: false and an error.


### Remove Category(ies)

**`RemoveItemResult<Category> RemoveCategory(string categoryId)`**

**`IEnumerable<RemoveItemResult<Category>> RemoveCategories(IEnumerable<string> categoryIds)`**

Removes a Category (or Categories) by Id from the internal collection. If Id is not in the internal collection then this does nothing.


### Add Brand(s)

**`AddItemResult AddBrand(Brand brand)`**

**`IEnumerable<AddItemResult> AddBrands(IEnumerable<Brand> brands)`**

Adds a Brand (or Brands) that you wish to send to PureClarity in a feed to an internal collection. If a Brand with the same Id has already been added then the AddItemResult will return with Success: false and an error.

### Remove Brand(s)

**`RemoveItemResult<Brand> RemoveBrand(string brandId)`**

**`IEnumerable<RemoveItemResult<Brand>> RemoveBrands(IEnumerable<string> brandIds)`**

Removes a Brand (or Brands) by Id from the internal collection. If Id is not in the internal collection then this does nothing.


### Add User(s)

**`AddItemResult AddUser(User user)`**

**`IEnumerable<AddItemResult> AddUsers(IEnumerable<User> Users)`**

Adds a User (or Users) that you wish to send to PureClarity in a feed to an internal collection. If a User with the same Id has already been added then the AddItemResult will return with Success: false and an error.

### Remove User(s)

**`RemoveItemResult<User> RemoveUser(string userId)`**

**`IEnumerable<RemoveItemResult<User>> RemoveUsers(IEnumerable<string> userIds)`**

Removes a User (or Users) by Id from the internal collection.  If Id is not in the internal collection then this does nothing.


## Validating


### Validate Feed

**`FeedValidationResult Validate()`**

Validates the feed and returns a validation response. The validation response contains whether the feed is valid and, if not valid, provides a list of Ids alongside specific error messages for the Ids for each item type (category/product/user/brand/orders).

This must be called before publishing to verify the feeds are valid. Any changes made to the collections after validation will require that this is called again before publishing.


## Publishing


### Publish Feed

**`PublishResult Publish()`**

**`Task<PublishResult> PublishAsync()`**

Publishes validated feeds to the PureClarity SFTP server and returns the publish result. An async version is provided and is the preferred usage. If PublishDeltas(Async) has already been called for this FeedManager instance then this call will not publish the product feed. Other available feeds will be published.

If validation has not yet been called an error will be returned.

### Publish Deltas

**`PublishDeltaResult PublishDeltas()`**

**`Task<PublishDeltaResult> PublishDeltasAsync()`**

Publishes validated deltas to the PureClarity delta API and returns the publish result alongside returned delta tokens. An async version is provided and is the preferred usage. If Publish(Async) has already been called for this FeedManager instance then this call will not publish the product deltas.

If validation has not yet been called an error will be returned.

Returns [PublishDeltaResult](models/publish-delta-result) on completion.


## Querying collection state

### Collection State

**`CollectionState<Product> GetProductCollectionState()`**

**`CollectionState<DeletedProductSku> GetDeletedProductCollectionState()`**

**`CollectionState<AccountPrice> GetAccountPriceCollectionState()`**

**`CollectionState<DeletedAccountPrice> GetDeletedAccountPriceCollectionState()`**

**`CollectionState<Category> GetCategoryCollectionState()`**

**`CollectionState<Brand> GetBrandCollectionState()`**

**`CollectionState<User> GetUserCollectionState()`**

Returns the state of the internal collection. This includes item count and a read only collection of the items added so far (items that did not return an error in the AddItemResult object);
