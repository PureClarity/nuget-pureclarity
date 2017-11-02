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
        private int _region;
        private string _accessKey;
        private string _secretKey;

        private ProductCollection _productCollection;
        private DeletedProductCollection _deletedProductCollection;
        private CategoryCollection _categoryCollection;
        private BrandCollection _brandCollection;
        private AccountPriceCollection _accountPriceCollection;
        private DeletedAccountPriceCollection _deletedAccountPriceCollection;
        private UserCollection _userCollection;

        private bool _productsPushed;


        public FeedManager(string accessKey, string secretKey, int region)
        {
            _accessKey = accessKey ?? throw new System.ArgumentNullException(nameof(accessKey));
            _secretKey = secretKey ?? throw new System.ArgumentNullException(nameof(secretKey));
            _region = region;

            _productCollection = new ProductCollection();
            _categoryCollection = new CategoryCollection();
            _brandCollection = new BrandCollection();
            _accountPriceCollection = new AccountPriceCollection();
            _userCollection = new UserCollection();
            _deletedProductCollection = new DeletedProductCollection();
            _deletedAccountPriceCollection = new DeletedAccountPriceCollection();
        }

        #region Add

        public AddItemResult AddProduct(Product product) => _productCollection.AddItem(product);
        public IEnumerable<AddItemResult> AddProducts(IEnumerable<Product> products) => _productCollection.AddItems(products);

        public AddItemResult AddDeletedProductSku(string productSku)
        {
            return _deletedProductCollection.AddItem(new DeletedProductSku(productSku));
        }

        public IEnumerable<AddItemResult> AddDeletedProductSkus(IEnumerable<string> productSkus)
        {
            return _deletedProductCollection.AddItems(productSkus.Select((sku) => { return new DeletedProductSku(sku); }));
        }

        public AddItemResult AddCategory(Category category) => _categoryCollection.AddItem(category);
        public IEnumerable<AddItemResult> AddCategories(IEnumerable<Category> categories) => _categoryCollection.AddItems(categories);

        public AddItemResult AddBrand(Brand brand) => _brandCollection.AddItem(brand);
        public IEnumerable<AddItemResult> AddBrands(IEnumerable<Brand> brands) => _brandCollection.AddItems(brands);

        public AddItemResult AddAccountPrice(AccountPrice accountPrice) => _accountPriceCollection.AddItem(accountPrice);
        public IEnumerable<AddItemResult> AddAccountPrices(IEnumerable<AccountPrice> accountPrices) => _accountPriceCollection.AddItems(accountPrices);

        public AddItemResult AddDeletedAccountPrice(DeletedAccountPrice deletedAccountPrice) => _deletedAccountPriceCollection.AddItem(deletedAccountPrice);
        public IEnumerable<AddItemResult> AddDeletedAccountPrices(IEnumerable<DeletedAccountPrice> deletedAccountPrices) => _deletedAccountPriceCollection.AddItems(deletedAccountPrices);

        public AddItemResult AddUser(User user) => _userCollection.AddItem(user);
        public IEnumerable<AddItemResult> AddUsers(IEnumerable<User> users) => _userCollection.AddItems(users);

        #endregion

        #region Remove

        public RemoveItemResult<Product> RemoveProduct(string productSku) => _productCollection.RemoveItemFromCollection(productSku);
        public IEnumerable<RemoveItemResult<Product>> RemoveProducts(IEnumerable<string> productSkus) => _productCollection.RemoveItemsFromCollection(productSkus);

        public RemoveItemResult<DeletedProductSku> RemoveDeletedProductSku(string productSku) => _deletedProductCollection.RemoveItemFromCollection(productSku);
        public IEnumerable<RemoveItemResult<DeletedProductSku>> RemoveDeletedProductSkus(IEnumerable<string> productSkus) => _deletedProductCollection.RemoveItemsFromCollection(productSkus);

        public RemoveItemResult<Category> RemoveCategory(string categoryId) => _categoryCollection.RemoveItemFromCollection(categoryId);
        public IEnumerable<RemoveItemResult<Category>> RemoveCategories(IEnumerable<string> categoryIds) => _categoryCollection.RemoveItemsFromCollection(categoryIds);

        public RemoveItemResult<Brand> RemoveBrand(string brandId) => _brandCollection.RemoveItemFromCollection(brandId);
        public IEnumerable<RemoveItemResult<Brand>> RemoveBrands(IEnumerable<string> brandIds) => _brandCollection.RemoveItemsFromCollection(brandIds);

        public RemoveItemResult<AccountPrice> RemoveAccountPrice(string accountPriceId) => _accountPriceCollection.RemoveItemFromCollection(accountPriceId);
        public IEnumerable<RemoveItemResult<AccountPrice>> RemoveAccountPrices(IEnumerable<string> accountPriceIds) => _accountPriceCollection.RemoveItemsFromCollection(accountPriceIds);

        public RemoveItemResult<DeletedAccountPrice> RemoveDeletedAccountPrice(string deletedAccountPriceId) => _deletedAccountPriceCollection.RemoveItemFromCollection(deletedAccountPriceId);
        public IEnumerable<RemoveItemResult<DeletedAccountPrice>> RemoveDeletedAccountPrices(IEnumerable<string> deletedAccountPriceIds) => _deletedAccountPriceCollection.RemoveItemsFromCollection(deletedAccountPriceIds);


        public RemoveItemResult<User> RemoveUser(string userId) => _userCollection.RemoveItemFromCollection(userId);
        public IEnumerable<RemoveItemResult<User>> RemoveUser(IEnumerable<string> userIds) => _userCollection.RemoveItemsFromCollection(userIds);

        #endregion

        #region Validate

        public FeedValidationResult Validate()
        {
            var validationResult = new FeedValidationResult();
            validationResult.ProductValidationResult = _productCollection.Validate();
            validationResult.CategoryValidationResult = _categoryCollection.Validate();
            //validationResult.BrandValidationResult = _brandCollection.Validate();
            validationResult.UserValidationResult = _userCollection.Validate();
            validationResult.Success = validationResult.ProductValidationResult.Success
            && validationResult.CategoryValidationResult.Success
            //&& validationResult.BrandValidationResult.Success;
            && validationResult.UserValidationResult.Success;

            return validationResult;
        }

        #endregion

        #region Publish

        public PublishResult Publish()
        {
            var publishManager = new PublishManager(_accessKey, _secretKey, _region);
            var publishResult = new PublishResult();

            if (!_productsPushed && _productCollection.GetCollectionState().ItemCount != 0)
            {
                publishResult.PublishProductFeedResult = publishManager.PublishProductFeed(_productCollection.GetItems(), _accountPriceCollection.GetItems()).Result;
                _productsPushed = true;
            }

            if (_categoryCollection.GetCollectionState().ItemCount != 0)
            {
                publishResult.PublishCategoryFeedResult = publishManager.PublishCategoryFeed(_categoryCollection.GetItems()).Result;
            }

            if (_userCollection.GetCollectionState().ItemCount != 0)
            {
                publishResult.PublishUserFeedResult = publishManager.PublishUserFeed(_userCollection.GetItems()).Result;
            }

            publishResult.Success = (publishResult.PublishProductFeedResult?.Success ?? true)
                                    && (publishResult.PublishCategoryFeedResult?.Success ?? true)
                                    && (publishResult.PublishUserFeedResult?.Success ?? true);

            return publishResult;
        }

        public async Task<PublishResult> PublishAsync()
        {
            var publishManager = new PublishManager(_accessKey, _secretKey, _region);
            var publishResult = new PublishResult();

            if (!_productsPushed && _productCollection.GetCollectionState().ItemCount != 0)
            {
                publishResult.PublishProductFeedResult = await publishManager.PublishProductFeed(_productCollection.GetItems(), _accountPriceCollection.GetItems());
                _productsPushed = true;
            }

            if (_categoryCollection.GetCollectionState().ItemCount != 0)
            {
                publishResult.PublishCategoryFeedResult = await publishManager.PublishCategoryFeed(_categoryCollection.GetItems());
            }

            if (_userCollection.GetCollectionState().ItemCount != 0)
            {
                publishResult.PublishUserFeedResult = await publishManager.PublishUserFeed(_userCollection.GetItems());
            }

            publishResult.Success = (publishResult.PublishProductFeedResult?.Success ?? true)
                                    && (publishResult.PublishCategoryFeedResult?.Success ?? true)
                                    && (publishResult.PublishUserFeedResult?.Success ?? true);

            return publishResult;
        }

        public PublishDeltaResult PublishDeltas()
        {
            var publishManager = new PublishManager(_accessKey, _secretKey, _region);
            var publishResult = new PublishDeltaResult();

            if (!_productsPushed)
            {
                var publishProductDeltas = publishManager.PublishProductDeltas(_productCollection.GetItems(), _deletedProductCollection.GetItems(), _accountPriceCollection.GetItems(), _deletedAccountPriceCollection.GetItems(), _accessKey).Result;
                publishResult = publishProductDeltas;
                _productsPushed = true;
            }

            return publishResult;
        }

        public async Task<PublishDeltaResult> PublishDeltasAsync()
        {
            var publishManager = new PublishManager(_accessKey, _secretKey, _region);
            var publishResult = new PublishDeltaResult();

            if (!_productsPushed)
            {
                publishResult = await publishManager.PublishProductDeltas(_productCollection.GetItems(), _deletedProductCollection.GetItems(), _accountPriceCollection.GetItems(), _deletedAccountPriceCollection.GetItems(), _accessKey);
                _productsPushed = true;
            }

            return publishResult;
        }



        #endregion

        #region CollectionState

        public CollectionState<Product> GetProductCollectionState() => _productCollection.GetCollectionState();
        public CollectionState<Category> GetCategoryCollectionState() => _categoryCollection.GetCollectionState();
        public CollectionState<Brand> GetBrandCollectionState() => _brandCollection.GetCollectionState();
        public CollectionState<User> GetUserCollectionState() => _userCollection.GetCollectionState();


        #endregion

    }
}