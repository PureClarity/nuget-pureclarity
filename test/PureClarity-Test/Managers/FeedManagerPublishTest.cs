using PureClarity.Managers;
using Xunit;

namespace PureClarity_Test
{
    public class FeedManagerPublishTest
    {
        /// <summary>
        /// Publishing reports failure on the result rather than throwing, so callers that only
        /// use try/catch would otherwise miss it.
        /// </summary>
        [Fact]
        public void ReportsUnvalidatedFeedsOnResultWithoutThrowing()
        {
            var feedManager = new FeedManager("access", "secret");

            var result = feedManager.Publish();

            Assert.False(result.Success);
            Assert.False(string.IsNullOrEmpty(result.Error));
        }

        [Fact]
        public async System.Threading.Tasks.Task ReportsUnvalidatedFeedsOnAsyncResultWithoutThrowing()
        {
            var feedManager = new FeedManager("access", "secret");

            var result = await feedManager.PublishAsync();

            Assert.False(result.Success);
            Assert.False(string.IsNullOrEmpty(result.Error));
        }
    }
}
