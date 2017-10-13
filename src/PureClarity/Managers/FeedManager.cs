using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PureClarity.Models;

namespace PureClarity.Managers
{
    public class FeedManager
    {
        private string _endpoint;
        private string _accessKey;
        private string _secretKey;

        private ProductCollection _productCollection;
        private DeletedProductCollection _deletedProductCollection;
        private CategoryCollection _categoryCollection;
        private BrandCollection _brandCollection;
        private UserCollection _userCollection;


        public FeedManager(string endpointUrl, string accessKey, string secretKey)
        {
            _endpoint = endpointUrl;
            _accessKey = accessKey;
            _secretKey = secretKey;

            _productCollection = new ProductCollection();
            _categoryCollection = new CategoryCollection();
            _brandCollection = new BrandCollection();
            _userCollection = new UserCollection();
            _deletedProductCollection = new DeletedProductCollection();
        }

        #region Add

        void AddProduct(Product product) => _productCollection.AddItem(product);
        void AddProducts(IEnumerable<Product> products) => _productCollection.AddItems(products);

        void AddDeletedProductSku(string productSku)
        {
            _deletedProductCollection.AddItem(new DeletedProductSku(productSku));
        }
        void AddDeletedProductSkus(IEnumerable<string> productSkus)
        {
            _deletedProductCollection.AddItems(productSkus.Select((sku) => { return new DeletedProductSku(sku); }));
        }

        void AddCategory(Category category) => _categoryCollection.AddItem(category);
        void AddCategories(IEnumerable<Category> categories) => _categoryCollection.AddItems(categories);

        void AddBrand(Brand brand) => _brandCollection.AddItem(brand);
        void AddBrands(IEnumerable<Brand> brands) => _brandCollection.AddItems(brands);

        void AddUser(User user) => _userCollection.AddItem(user);
        void AddUsers(IEnumerable<User> users) => _userCollection.AddItems(users);

        #endregion

        #region Remove

        void RemoveProduct(string productSku) => _productCollection.RemoveItemFromCollection(productSku);
        void RemoveProducts(IEnumerable<string> productSkus) => _productCollection.RemoveItemsFromCollection(productSkus);

        void RemoveDeletedProductSku(string productSku) => _deletedProductCollection.RemoveItemFromCollection(productSku);
        void RemoveDeletedProductSkus(IEnumerable<string> productSkus) => _deletedProductCollection.RemoveItemsFromCollection(productSkus);

        void RemoveCategory(string categoryId) => _categoryCollection.RemoveItemFromCollection(categoryId);
        void RemoveCategories(IEnumerable<string> categoryIds) => _categoryCollection.RemoveItemsFromCollection(categoryIds);

        void RemoveBrand(string brandId) => _brandCollection.RemoveItemFromCollection(brandId);
        void RemoveBrands(IEnumerable<string> brandIds) => _brandCollection.RemoveItemsFromCollection(brandIds);

        #endregion

        #region Validate

        #endregion

    }
}