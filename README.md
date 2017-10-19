# nuget-pureclarity
The PureClarity SDK .Net Standard repo.

# API


## Feeds

### FeedManager Constructor

**`new FeedManager(EndpointURL, AccessKey, SecretKey)`**

Sets the endpoint URL, Access Key and Secret Key to use when making calls to PureClarity


### Add Product(s)
 
**`AddItemResult AddProduct(Product)`**

**`IEnumerable<AddItemResult> AddProducts(Products)`**

Adds a product (or products) that you wish to send to PureClarity in a feed to an internal collection. If a product with the same Sku has already been added then the new product will overwrite it.


### Remove Product(s)

**`RemoveItemResult<Product> RemoveProduct(Sku)`**

**`IEnumerable<RemoveItemResult<Product>> RemoveProducts(Skus)`**

Removes a product (or products) by Sku from the internal collection. If Sku is not in the internal collection then this does nothing.


### Add Category(ies)
 
**`AddItemResult AddCategory(Category)`**

**`IEnumerable<AddItemResult> AddCategories(Categories)`**

Adds a Category (or Categories) that you wish to send to PureClarity in a feed to an internal collection. If a Category with the same Id has already been added then the new Category will overwrite it.


### Remove Category(ies)

**`RemoveItemResult<Category> RemoveCategory(CategoryId)`**

**`IEnumerable<RemoveItemResult<Category>> RemoveCategories(CategoryIds)`**

Removes a Category (or Categories) by Id from the internal collection. If Id is not in the internal collection then this does nothing.


### Add Brand(s)

**`AddItemResult AddBrand(Brand)`**

**`IEnumerable<AddItemResult> AddBrands(Brands)`**

Adds a Brand (or Brands) that you wish to send to PureClarity in a feed to an internal collection. If a Brand with the same Id has already been added then the new Brand will overwrite it.

### Remove Brand(s)

**`RemoveItemResult<Brand> RemoveBrand(BrandId)`**

**`IEnumerable<RemoveItemResult<Brand>> RemoveBrands(BrandIds)`**

Removes a Brand (or Brands) by Id from the internal collection. If Id is not in the internal collection then this does nothing.


### Add User(s)

**`AddItemResult AddUser(User)`**

**`IEnumerable<AddItemResult> AddUsers(Users)`**

Adds a User (or Users) that you wish to send to PureClarity in a feed to an internal collection. If a User with the same Id has already been added then the new User will overwrite it.

### Remove User(s)

**`RemoveItemResult<User> RemoveUser(UserId)`**

**`IEnumerable<RemoveItemResult<User>> RemoveUsers(UserIds)`**

Removes a User (or Users) by Id from the internal collection.  If Id is not in the internal collection then this does nothing.


### Validate Feed

**`FeedValidationResult Validate()`**

Validates the feed and returns a validation response. The validation response contains whether the feed is valid and, if not valid, provides a list of Ids alongside specific error messages for the Ids for each item type (category/product/user/brand/orders)

### Publish Feed

**`PublishFeedResult PublishProductFeed()`**

Publishes a validated product feed to the PureClarity SFTP server and returns the publish result.

