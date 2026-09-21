using System.Collections.Generic;
using System.Threading.Tasks;
using PureClarity.Models;

namespace PureClarity.Managers
{
    internal interface IPublishManager
    {
        Task<PublishFeedResult> PublishProductFeed(IEnumerable<Product> products, IEnumerable<AccountPrice> accountPrices);

        Task<PublishDeltaResult> PublishProductDeltas(IEnumerable<Product> products, IEnumerable<DeletedProductSku> deletedProducts, IEnumerable<AccountPrice> accountPrices, IEnumerable<DeletedAccountPrice> deletedAccountPrices, string accessKey);

        Task<PublishFeedResult> PublishCategoryFeed(IEnumerable<Category> categories);

        Task<PublishFeedResult> PublishBrandFeed(IEnumerable<Brand> brands);

        Task<PublishFeedResult> PublishUserFeed(IEnumerable<User> users);
    }
}
