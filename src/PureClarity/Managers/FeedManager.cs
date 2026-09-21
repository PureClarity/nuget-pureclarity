using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PureClarity.Collections;
using PureClarity.Models;

namespace PureClarity.Managers
{
    public class FeedManager
    {
        private string _accessKey;
        private string _secretKey;
        private IReadOnlyList<string> _sftpHostKeyFingerprints;
        private readonly IPublishManager _publishManager;

        private ProductCollection _productCollection;
        private DeletedProductCollection _deletedProductCollection;
        private CategoryCollection _categoryCollection;
        private BrandCollection _brandCollection;
        private AccountPriceCollection _accountPriceCollection;
        private DeletedAccountPriceCollection _deletedAccountPriceCollection;
        private UserCollection _userCollection;

        private bool _productsPushed;

        private bool _feedsValid = false;

        /// <summary>
        /// Initialises the Feed Manager
        /// </summary>
        /// <param name="accessKey">The access key identifies the client</param>
        /// <param name="secretKey">The secret key is used for authentication when publishing a feed</param>
        /// <param name="region">Ignored. All PureClarity traffic is served by a single set of endpoints.</param>
        [Obsolete("The region argument is ignored. Use FeedManager(accessKey, secretKey) instead.")]
        public FeedManager(string accessKey, string secretKey, int region)
            : this(accessKey, secretKey, (IReadOnlyList<string>)null)
        {
        }

        /// <summary>
        /// Initialises the Feed Manager
        /// </summary>
        /// <param name="accessKey">The access key identifies the client</param>
        /// <param name="secretKey">The secret key is used for authentication when publishing a feed</param>
        /// <param name="region">Ignored. All PureClarity traffic is served by a single set of endpoints.</param>
        /// <param name="sftpHostKeyFingerprints">Expected SHA256 host key fingerprints for the SFTP endpoint, overriding the built-in values. Supply more than one to span a host key rotation.</param>
        [Obsolete("The region argument is ignored. Use FeedManager(accessKey, secretKey, sftpHostKeyFingerprints) instead.")]
        public FeedManager(string accessKey, string secretKey, int region, IReadOnlyList<string> sftpHostKeyFingerprints)
            : this(accessKey, secretKey, sftpHostKeyFingerprints)
        {
        }

        /// <summary>
        /// Initialises the Feed Manager
        /// </summary>
        /// <param name="accessKey">The access key identifies the client</param>
        /// <param name="secretKey">The secret key is used for authentication when publishing a feed</param>
        public FeedManager(string accessKey, string secretKey)
            : this(accessKey, secretKey, (IReadOnlyList<string>)null)
        {
        }

        /// <summary>
        /// Initialises the Feed Manager
        /// </summary>
        /// <param name="accessKey">The access key identifies the client</param>
        /// <param name="secretKey">The secret key is used for authentication when publishing a feed</param>
        /// <param name="sftpHostKeyFingerprints">Expected SHA256 host key fingerprints for the SFTP endpoint, overriding the built-in values. Supply more than one to span a host key rotation.</param>
        public FeedManager(string accessKey, string secretKey, IReadOnlyList<string> sftpHostKeyFingerprints)
            : this(accessKey, secretKey, sftpHostKeyFingerprints, null)
        {
        }

        internal FeedManager(string accessKey, string secretKey, IReadOnlyList<string> sftpHostKeyFingerprints, IPublishManager publishManager)
        {
            _accessKey = accessKey ?? throw new System.ArgumentNullException(nameof(accessKey));
            _secretKey = secretKey ?? throw new System.ArgumentNullException(nameof(secretKey));
            _sftpHostKeyFingerprints = sftpHostKeyFingerprints;
            _publishManager = publishManager ?? new PublishManager(_accessKey, _secretKey, _sftpHostKeyFingerprints);

            _productCollection = new ProductCollection();
            _categoryCollection = new CategoryCollection();
            _brandCollection = new BrandCollection();
            _accountPriceCollection = new AccountPriceCollection();
            _userCollection = new UserCollection();
            _deletedProductCollection = new DeletedProductCollection();
            _deletedAccountPriceCollection = new DeletedAccountPriceCollection();
        }

        #region Add

        public AddItemResult AddProduct(Product product)
        {
            _feedsValid = false;
            return _productCollection.AddItem(product);
        }
        public IEnumerable<AddItemResult> AddProducts(IEnumerable<Product> products)
        {
            _feedsValid = false;
            return _productCollection.AddItems(products);
        }

        public AddItemResult AddDeletedProductSku(string productSku)
        {
            _feedsValid = false;
            return _deletedProductCollection.AddItem(new DeletedProductSku(productSku));
        }

        public IEnumerable<AddItemResult> AddDeletedProductSkus(IEnumerable<string> productSkus)
        {
            _feedsValid = false;
            return _deletedProductCollection.AddItems(productSkus.Select((sku) => { return new DeletedProductSku(sku); }));
        }

        public AddItemResult AddCategory(Category category)
        {
            _feedsValid = false;
            return _categoryCollection.AddItem(category);
        }
        public IEnumerable<AddItemResult> AddCategories(IEnumerable<Category> categories)
        {
            _feedsValid = false;
            return _categoryCollection.AddItems(categories);
        }

        public AddItemResult AddBrand(Brand brand)
        {
            _feedsValid = false;
            return _brandCollection.AddItem(brand);
        }
        public IEnumerable<AddItemResult> AddBrands(IEnumerable<Brand> brands)
        {
            _feedsValid = false;
            return _brandCollection.AddItems(brands);
        }

        public AddItemResult AddAccountPrice(AccountPrice accountPrice)
        {
            _feedsValid = false;
            return _accountPriceCollection.AddItem(accountPrice);
        }
        public IEnumerable<AddItemResult> AddAccountPrices(IEnumerable<AccountPrice> accountPrices)
        {
            _feedsValid = false;
            return _accountPriceCollection.AddItems(accountPrices);
        }

        public AddItemResult AddDeletedAccountPrice(DeletedAccountPrice deletedAccountPrice)
        {
            _feedsValid = false;
            return _deletedAccountPriceCollection.AddItem(deletedAccountPrice);
        }
        public IEnumerable<AddItemResult> AddDeletedAccountPrices(IEnumerable<DeletedAccountPrice> deletedAccountPrices)
        {
            _feedsValid = false;
            return _deletedAccountPriceCollection.AddItems(deletedAccountPrices);
        }

        public AddItemResult AddUser(User user)
        {
            _feedsValid = false;
            return _userCollection.AddItem(user);
        }
        public IEnumerable<AddItemResult> AddUsers(IEnumerable<User> users)
        {
            _feedsValid = false;
            return _userCollection.AddItems(users);
        }

        #endregion

        #region Remove

        public RemoveItemResult<Product> RemoveProduct(string productSku)
        {
            _feedsValid = false;
            return _productCollection.RemoveItemFromCollection(productSku);
        }
        public IEnumerable<RemoveItemResult<Product>> RemoveProducts(IEnumerable<string> productSkus)
        {
            _feedsValid = false;
            return _productCollection.RemoveItemsFromCollection(productSkus);
        }

        public RemoveItemResult<DeletedProductSku> RemoveDeletedProductSku(string productSku)
        {
            _feedsValid = false;
            return _deletedProductCollection.RemoveItemFromCollection(productSku);
        }
        public IEnumerable<RemoveItemResult<DeletedProductSku>> RemoveDeletedProductSkus(IEnumerable<string> productSkus)
        {
            _feedsValid = false;
            return _deletedProductCollection.RemoveItemsFromCollection(productSkus);
        }

        public RemoveItemResult<Category> RemoveCategory(string categoryId)
        {
            _feedsValid = false;
            return _categoryCollection.RemoveItemFromCollection(categoryId);
        }
        public IEnumerable<RemoveItemResult<Category>> RemoveCategories(IEnumerable<string> categoryIds)
        {
            _feedsValid = false;
            return _categoryCollection.RemoveItemsFromCollection(categoryIds);
        }

        public RemoveItemResult<Brand> RemoveBrand(string brandId)
        {
            _feedsValid = false;
            return _brandCollection.RemoveItemFromCollection(brandId);
        }
        public IEnumerable<RemoveItemResult<Brand>> RemoveBrands(IEnumerable<string> brandIds)
        {
            _feedsValid = false;
            return _brandCollection.RemoveItemsFromCollection(brandIds);
        }

        public RemoveItemResult<AccountPrice> RemoveAccountPrice(string accountPriceId)
        {
            _feedsValid = false;
            return _accountPriceCollection.RemoveItemFromCollection(accountPriceId);
        }
        public RemoveItemResult<AccountPrice> RemoveAccountPrice(string accountId, string sku)
        {
            _feedsValid = false;
            return _accountPriceCollection.RemoveItemFromCollection($"{accountId}|{sku}");
        }
        public IEnumerable<RemoveItemResult<AccountPrice>> RemoveAccountPrices(IEnumerable<string> accountPriceIds)
        {
            _feedsValid = false;
            return _accountPriceCollection.RemoveItemsFromCollection(accountPriceIds);
        }

        public RemoveItemResult<DeletedAccountPrice> RemoveDeletedAccountPrice(string deletedAccountPriceId)
        {
            _feedsValid = false;
            return _deletedAccountPriceCollection.RemoveItemFromCollection(deletedAccountPriceId);
        }
        public RemoveItemResult<DeletedAccountPrice> RemoveDeletedAccountPrice(string accountId, string sku)
        {
            _feedsValid = false;
            return _deletedAccountPriceCollection.RemoveItemFromCollection($"{accountId}|{sku}");
        }
        public IEnumerable<RemoveItemResult<DeletedAccountPrice>> RemoveDeletedAccountPrices(IEnumerable<string> deletedAccountPriceIds)
        {
            _feedsValid = false;
            return _deletedAccountPriceCollection.RemoveItemsFromCollection(deletedAccountPriceIds);
        }


        public RemoveItemResult<User> RemoveUser(string userId)
        {
            _feedsValid = false;
            return _userCollection.RemoveItemFromCollection(userId);
        }
        public IEnumerable<RemoveItemResult<User>> RemoveUser(IEnumerable<string> userIds)
        {
            _feedsValid = false;
            return _userCollection.RemoveItemsFromCollection(userIds);
        }

        #endregion

        #region Validate

        public ValidationResult Validate()
        {
            var validationResult = new ValidationResult();
            validationResult.ProductValidationResult = _productCollection.Validate();
            validationResult.AccountPriceValidationResult = _accountPriceCollection.Validate();
            validationResult.CategoryValidationResult = _categoryCollection.Validate();
            validationResult.BrandValidationResult = _brandCollection.Validate();
            validationResult.UserValidationResult = _userCollection.Validate();

            validationResult.Success = validationResult.ProductValidationResult.Success
            && validationResult.AccountPriceValidationResult.Success
            && validationResult.CategoryValidationResult.Success            
            && validationResult.BrandValidationResult.Success
            && validationResult.UserValidationResult.Success;

            _feedsValid = validationResult.Success;

            return validationResult;
        }

        #endregion

        #region Publish

        public PublishResult Publish()
        {
            if (!_feedsValid)
            {
                return new PublishResult { Success = false, Error = "Feeds not yet successfully validated" };
            }

            var publishResult = new PublishResult();

            if (!_productsPushed && _productCollection.GetCollectionState().ItemCount != 0)
            {
                publishResult.PublishProductFeedResult = _publishManager.PublishProductFeed(_productCollection.GetItems(), _accountPriceCollection.GetItems()).Result;
                _productsPushed = publishResult.PublishProductFeedResult.Success;
            }

            if (_categoryCollection.GetCollectionState().ItemCount != 0)
            {
                publishResult.PublishCategoryFeedResult = _publishManager.PublishCategoryFeed(_categoryCollection.GetItems()).Result;
            }

            if (_brandCollection.GetCollectionState().ItemCount != 0)
            {
                publishResult.PublishBrandFeedResult = _publishManager.PublishBrandFeed(_brandCollection.GetItems()).Result;
            }

            if (_userCollection.GetCollectionState().ItemCount != 0)
            {
                publishResult.PublishUserFeedResult = _publishManager.PublishUserFeed(_userCollection.GetItems()).Result;
            }

            publishResult.Success = (publishResult.PublishProductFeedResult?.Success ?? true)
                                    && (publishResult.PublishCategoryFeedResult?.Success ?? true)
                                    && (publishResult.PublishBrandFeedResult?.Success ?? true)
                                    && (publishResult.PublishUserFeedResult?.Success ?? true);

            return publishResult;
        }

        public async Task<PublishResult> PublishAsync()
        {
            if (!_feedsValid)
            {
                return new PublishResult { Success = false, Error = "Feeds not yet successfully validated" };
            }

            var publishResult = new PublishResult();

            if (!_productsPushed && _productCollection.GetCollectionState().ItemCount != 0)
            {
                publishResult.PublishProductFeedResult = await _publishManager.PublishProductFeed(_productCollection.GetItems(), _accountPriceCollection.GetItems());
                _productsPushed = publishResult.PublishProductFeedResult.Success;
            }

            if (_categoryCollection.GetCollectionState().ItemCount != 0)
            {
                publishResult.PublishCategoryFeedResult = await _publishManager.PublishCategoryFeed(_categoryCollection.GetItems());
            }

            if (_brandCollection.GetCollectionState().ItemCount != 0)
            {
                publishResult.PublishBrandFeedResult = await _publishManager.PublishBrandFeed(_brandCollection.GetItems());
            }

            if (_userCollection.GetCollectionState().ItemCount != 0)
            {
                publishResult.PublishUserFeedResult = await _publishManager.PublishUserFeed(_userCollection.GetItems());
            }

            publishResult.Success = (publishResult.PublishProductFeedResult?.Success ?? true)
                                    && (publishResult.PublishCategoryFeedResult?.Success ?? true)
                                    && (publishResult.PublishBrandFeedResult?.Success ?? true)
                                    && (publishResult.PublishUserFeedResult?.Success ?? true);

            return publishResult;
        }

        public PublishDeltaResult PublishDeltas()
        {
            if (!_feedsValid)
            {
                return new PublishDeltaResult { Success = false, Error = "Feeds not yet successfully validated" };
            }

            var publishResult = new PublishDeltaResult();

            if (!_productsPushed)
            {
                var publishProductDeltas = _publishManager.PublishProductDeltas(_productCollection.GetItems(), _deletedProductCollection.GetItems(), _accountPriceCollection.GetItems(), _deletedAccountPriceCollection.GetItems(), _accessKey).Result;
                publishResult = publishProductDeltas;
                _productsPushed = publishProductDeltas.Success;
            }

            return publishResult;
        }

        public async Task<PublishDeltaResult> PublishDeltasAsync()
        {
            if (!_feedsValid)
            {
                return new PublishDeltaResult { Success = false, Error = "Feeds not yet successfully validated" };
            }

            var publishResult = new PublishDeltaResult();

            if (!_productsPushed)
            {
                publishResult = await _publishManager.PublishProductDeltas(_productCollection.GetItems(), _deletedProductCollection.GetItems(), _accountPriceCollection.GetItems(), _deletedAccountPriceCollection.GetItems(), _accessKey);
                _productsPushed = publishResult.Success;
            }

            return publishResult;
        }

        #endregion

        #region CollectionState

        public CollectionState<Product> GetProductCollectionState() => _productCollection.GetCollectionState();
        public CollectionState<DeletedProductSku> GetDeletedProductCollectionState() => _deletedProductCollection.GetCollectionState();
        public CollectionState<AccountPrice> GetAccountPriceCollectionState() => _accountPriceCollection.GetCollectionState();
        public CollectionState<DeletedAccountPrice> GetDeletedAccountPriceCollectionState() => _deletedAccountPriceCollection.GetCollectionState();
        public CollectionState<Category> GetCategoryCollectionState() => _categoryCollection.GetCollectionState();
        public CollectionState<Brand> GetBrandCollectionState() => _brandCollection.GetCollectionState();
        public CollectionState<User> GetUserCollectionState() => _userCollection.GetCollectionState();


        #endregion

    }
}