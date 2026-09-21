using System.Collections.Generic;
using System.Threading.Tasks;
using PureClarity.Managers;
using PureClarity.Models;
using Xunit;

namespace PureClarity_Test
{
    public class FeedManagerPublishRetryTest
    {
        private static FeedManager CreateFeedManager(FakePublishManager publisher)
        {
            return new FeedManager("access", "secret", null, publisher);
        }

        private static Product CreateProduct(string sku)
        {
            var product = new Product(sku, sku, sku, sku, sku, new List<string> { sku });
            product.Prices.Add(new Price(10m, "GBP"));
            return product;
        }

        private static FeedManager WithProducts(FakePublishManager publisher)
        {
            var feedManager = CreateFeedManager(publisher);
            feedManager.AddProduct(CreateProduct("sku-1"));
            Assert.True(feedManager.Validate().Success);
            return feedManager;
        }

        #region Product feed retry

        /// <summary>
        /// A failed product upload must not latch _productsPushed, otherwise the retry silently
        /// skips the upload and reports success with nothing sent.
        /// </summary>
        [Fact]
        public void RetriesProductFeedAfterFailure()
        {
            var publisher = new FakePublishManager { ProductFeedSucceeds = false };
            var feedManager = WithProducts(publisher);

            var first = feedManager.Publish();
            Assert.False(first.Success);

            var second = feedManager.Publish();

            Assert.Equal(2, publisher.ProductFeedCalls);
            Assert.NotNull(second.PublishProductFeedResult);
            Assert.False(second.Success);
        }

        [Fact]
        public async Task RetriesProductFeedAfterFailureAsync()
        {
            var publisher = new FakePublishManager { ProductFeedSucceeds = false };
            var feedManager = WithProducts(publisher);

            Assert.False((await feedManager.PublishAsync()).Success);
            var second = await feedManager.PublishAsync();

            Assert.Equal(2, publisher.ProductFeedCalls);
            Assert.NotNull(second.PublishProductFeedResult);
            Assert.False(second.Success);
        }

        [Fact]
        public void DoesNotRepublishProductFeedAfterSuccess()
        {
            var publisher = new FakePublishManager();
            var feedManager = WithProducts(publisher);

            Assert.True(feedManager.Publish().Success);
            feedManager.Publish();

            Assert.Equal(1, publisher.ProductFeedCalls);
        }

        /// <summary>
        /// The bug this guards: a skipped product upload leaves the result null, and a null result
        /// was treated as success by the aggregation.
        /// </summary>
        [Fact]
        public void DoesNotReportSuccessWhenProductUploadFailedAndNothingElseWasSent()
        {
            var publisher = new FakePublishManager { ProductFeedSucceeds = false };
            var feedManager = WithProducts(publisher);

            feedManager.Publish();
            var retry = feedManager.Publish();

            Assert.False(retry.Success);
        }

        #endregion

        #region Success aggregation

        [Fact]
        public void ReportsFailureWhenBrandFeedFails()
        {
            var publisher = new FakePublishManager { BrandFeedSucceeds = false };
            var feedManager = CreateFeedManager(publisher);
            feedManager.AddBrand(new Brand("brand-1"));
            Assert.True(feedManager.Validate().Success);

            var result = feedManager.Publish();

            Assert.Equal(1, publisher.BrandFeedCalls);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task ReportsFailureWhenBrandFeedFailsAsync()
        {
            var publisher = new FakePublishManager { BrandFeedSucceeds = false };
            var feedManager = CreateFeedManager(publisher);
            feedManager.AddBrand(new Brand("brand-1"));
            Assert.True(feedManager.Validate().Success);

            var result = await feedManager.PublishAsync();

            Assert.Equal(1, publisher.BrandFeedCalls);
            Assert.False(result.Success);
        }

        /// <summary>
        /// PublishAsync previously skipped brands entirely.
        /// </summary>
        [Fact]
        public async Task PublishesBrandFeedAsynchronously()
        {
            var publisher = new FakePublishManager();
            var feedManager = CreateFeedManager(publisher);
            feedManager.AddBrand(new Brand("brand-1"));
            feedManager.Validate();

            await feedManager.PublishAsync();

            Assert.Equal(1, publisher.BrandFeedCalls);
        }

        [Fact]
        public void ReportsFailureWhenCategoryFeedFails()
        {
            var publisher = new FakePublishManager { CategoryFeedSucceeds = false };
            var feedManager = CreateFeedManager(publisher);
            feedManager.AddCategory(new Category("cat-1", "Cat 1", "link"));
            Assert.True(feedManager.Validate().Success);

            Assert.False(feedManager.Publish().Success);
        }

        [Fact]
        public void ReportsFailureWhenUserFeedFails()
        {
            var publisher = new FakePublishManager { UserFeedSucceeds = false };
            var feedManager = CreateFeedManager(publisher);
            feedManager.AddUser(new User("user-1"));
            Assert.True(feedManager.Validate().Success);

            Assert.False(feedManager.Publish().Success);
        }

        #endregion

        #region Delta retry

        [Fact]
        public void RetriesDeltasAfterFailure()
        {
            var publisher = new FakePublishManager { DeltasSucceed = false };
            var feedManager = WithProducts(publisher);

            Assert.False(feedManager.PublishDeltas().Success);
            var second = feedManager.PublishDeltas();

            Assert.Equal(2, publisher.DeltaCalls);
            Assert.False(second.Success);
            Assert.False(string.IsNullOrEmpty(second.Error));
        }

        [Fact]
        public async Task RetriesDeltasAfterFailureAsync()
        {
            var publisher = new FakePublishManager { DeltasSucceed = false };
            var feedManager = WithProducts(publisher);

            Assert.False((await feedManager.PublishDeltasAsync()).Success);
            var second = await feedManager.PublishDeltasAsync();

            Assert.Equal(2, publisher.DeltaCalls);
            Assert.False(second.Success);
        }

        [Fact]
        public void DoesNotResendDeltasAfterSuccess()
        {
            var publisher = new FakePublishManager();
            var feedManager = WithProducts(publisher);

            Assert.True(feedManager.PublishDeltas().Success);
            feedManager.PublishDeltas();

            Assert.Equal(1, publisher.DeltaCalls);
        }

        #endregion

        [Fact]
        public void DoesNotPublishWhenFeedsNotValidated()
        {
            var publisher = new FakePublishManager();
            var feedManager = CreateFeedManager(publisher);
            feedManager.AddProduct(CreateProduct("sku-1"));

            var result = feedManager.Publish();

            Assert.False(result.Success);
            Assert.Equal(0, publisher.ProductFeedCalls);
        }
    }
}
