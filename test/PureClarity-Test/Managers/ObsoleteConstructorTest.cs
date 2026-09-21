using System;
using PureClarity.Managers;
using Xunit;

namespace PureClarity_Test
{
    /// <summary>
    /// The region-taking constructors are obsolete but must keep working for existing callers.
    /// </summary>
    public class ObsoleteConstructorTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(4)]
        [InlineData(int.MaxValue)]
        public void FeedManagerStillAcceptsAnyRegion(int region)
        {
#pragma warning disable CS0618
            Assert.NotNull(new FeedManager("access", "secret", region));
            Assert.NotNull(new FeedManager("access", "secret", region, new[] { "SHA256:abc" }));
#pragma warning restore CS0618
        }

        [Theory]
        [InlineData(0)]
        [InlineData(4)]
        [InlineData(int.MaxValue)]
        public void QueryTokenManagerStillAcceptsAnyRegion(int region)
        {
#pragma warning disable CS0618
            Assert.NotNull(new QueryTokenManager("access", region));
#pragma warning restore CS0618
        }

        [Fact]
        public void RegionFreeConstructorsExist()
        {
            Assert.NotNull(new FeedManager("access", "secret"));
            Assert.NotNull(new FeedManager("access", "secret", new[] { "SHA256:abc" }));
            Assert.NotNull(new QueryTokenManager("access"));
        }

        [Fact]
        public void StillRejectsNullKeys()
        {
            Assert.Throws<ArgumentNullException>(() => new FeedManager(null, "secret"));
            Assert.Throws<ArgumentNullException>(() => new FeedManager("access", null));
            Assert.Throws<ArgumentNullException>(() => new QueryTokenManager(null));
        }
    }
}
