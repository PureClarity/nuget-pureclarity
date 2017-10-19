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
        private Region _region;
        private string _endpoint;
        private string _accessKey;
        private string _secretKey;

        private ProductCollection _productCollection;
        private DeletedProductCollection _deletedProductCollection;
        private CategoryCollection _categoryCollection;
        private BrandCollection _brandCollection;
        private UserCollection _userCollection;


        public FeedManager(string endpointUrl, string accessKey, string secretKey, Region region)
        {
            _endpoint = endpointUrl ?? throw new System.ArgumentNullException(nameof(endpointUrl));
            _accessKey = accessKey ?? throw new System.ArgumentNullException(nameof(accessKey));
            _secretKey = secretKey ?? throw new System.ArgumentNullException(nameof(secretKey));
            _region = region;

            _productCollection = new ProductCollection();
            _categoryCollection = new CategoryCollection();
            _brandCollection = new BrandCollection();
            _userCollection = new UserCollection();
            _deletedProductCollection = new DeletedProductCollection();
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

        public RemoveItemResult<User> RemoveUser(string userId) => _userCollection.RemoveItemFromCollection(userId);
        public IEnumerable<RemoveItemResult<User>> RemoveUser(IEnumerable<string> userIds) => _userCollection.RemoveItemsFromCollection(userIds);

        #endregion

        #region Validate

        public FeedValidationResult Validate()
        {
            var validationResult = new FeedValidationResult();
            validationResult.ProductValidationResult = _productCollection.Validate();
            /* validationResult.CategoryValidationResult = _categoryCollection.Validate();
            validationResult.BrandValidationResult = _brandCollection.Validate();
            validationResult.UserValidationResult = _userCollection.Validate(); */
            validationResult.Success = validationResult.ProductValidationResult.Success;
            /* && validationResult.CategoryValidationResult.Success
            && validationResult.BrandValidationResult.Success
            && validationResult.UserValidationResult.Success; */

            return validationResult;
        }

        #endregion

        #region Publish

        public async Task<PublishFeedResult> Publish()
        {
            var feed = ConversionManager.ProcessProductFeed(_productCollection.GetItems());
            var publishManager = new PublishManager(_accessKey,_secretKey, _region);
            var publishProducts = publishManager.PublishProductFeed(feed);

            await Task.WhenAll(publishProducts);

            return await publishProducts;
        }

        #endregion

        #region CollectionState

        public CollectionState GetProductCollectionState() => _productCollection.GetCollectionState();
        public CollectionState GetCategoryCollectionState() => _categoryCollection.GetCollectionState();
        public CollectionState GetBrandCollectionState() => _brandCollection.GetCollectionState();
        public CollectionState GetUserCollectionState() => _userCollection.GetCollectionState();


        #endregion

    }
}