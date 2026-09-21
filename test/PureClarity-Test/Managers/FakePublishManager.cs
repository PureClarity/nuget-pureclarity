using System.Collections.Generic;
using System.Threading.Tasks;
using PureClarity.Managers;
using PureClarity.Models;

namespace PureClarity_Test
{
    internal class FakePublishManager : IPublishManager
    {
        public bool ProductFeedSucceeds = true;
        public bool CategoryFeedSucceeds = true;
        public bool BrandFeedSucceeds = true;
        public bool UserFeedSucceeds = true;
        public bool DeltasSucceed = true;

        public int ProductFeedCalls;
        public int CategoryFeedCalls;
        public int BrandFeedCalls;
        public int UserFeedCalls;
        public int DeltaCalls;

        public Task<PublishFeedResult> PublishProductFeed(IEnumerable<Product> products, IEnumerable<AccountPrice> accountPrices)
        {
            ProductFeedCalls++;
            return Task.FromResult(Result(ProductFeedSucceeds));
        }

        public Task<PublishDeltaResult> PublishProductDeltas(IEnumerable<Product> products, IEnumerable<DeletedProductSku> deletedProducts, IEnumerable<AccountPrice> accountPrices, IEnumerable<DeletedAccountPrice> deletedAccountPrices, string accessKey)
        {
            DeltaCalls++;
            return Task.FromResult(new PublishDeltaResult { Success = DeltasSucceed, Error = DeltasSucceed ? null : "delta failed" });
        }

        public Task<PublishFeedResult> PublishCategoryFeed(IEnumerable<Category> categories)
        {
            CategoryFeedCalls++;
            return Task.FromResult(Result(CategoryFeedSucceeds));
        }

        public Task<PublishFeedResult> PublishBrandFeed(IEnumerable<Brand> brands)
        {
            BrandFeedCalls++;
            return Task.FromResult(Result(BrandFeedSucceeds));
        }

        public Task<PublishFeedResult> PublishUserFeed(IEnumerable<User> users)
        {
            UserFeedCalls++;
            return Task.FromResult(Result(UserFeedSucceeds));
        }

        private static PublishFeedResult Result(bool success)
        {
            return new PublishFeedResult { Success = success, Error = success ? null : "host key could not be verified" };
        }
    }
}
