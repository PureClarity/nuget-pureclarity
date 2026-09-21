using PureClarity.Helpers;
using Xunit;

namespace PureClarity_Test
{
    public class HostKeyFingerprintTest
    {
        private const string Actual = "abcDEF123+/xyzABCdefGHIjklMNOpqrSTUvwx012";

        [Fact]
        public void MatchesExactFingerprint()
        {
            Assert.True(HostKeyFingerprint.Matches(Actual, new[] { Actual }));
        }

        [Fact]
        public void MatchesWhenExpectedCarriesShaPrefix()
        {
            Assert.True(HostKeyFingerprint.Matches(Actual, new[] { "SHA256:" + Actual }));
        }

        [Fact]
        public void MatchesWhenPaddingDiffers()
        {
            Assert.True(HostKeyFingerprint.Matches(Actual + "=", new[] { Actual }));
        }

        [Fact]
        public void MatchesAnyOneOfSeveralExpectedFingerprints()
        {
            Assert.True(HostKeyFingerprint.Matches(Actual, new[] { "someOtherFingerprint", Actual }));
        }

        [Fact]
        public void DoesNotMatchDifferentFingerprint()
        {
            Assert.False(HostKeyFingerprint.Matches(Actual, new[] { "someOtherFingerprint" }));
        }

        /// <summary>
        /// Base64 is case sensitive, so a case-insensitive comparison would accept a different key.
        /// </summary>
        [Fact]
        public void DoesNotMatchOnDifferingCase()
        {
            Assert.False(HostKeyFingerprint.Matches(Actual.ToUpperInvariant(), new[] { Actual.ToLowerInvariant() }));
        }

        [Fact]
        public void DoesNotMatchWhenNoFingerprintsExpected()
        {
            Assert.False(HostKeyFingerprint.Matches(Actual, new string[0]));
            Assert.False(HostKeyFingerprint.Matches(Actual, null));
        }

        /// <summary>
        /// A blank expected entry must never be treated as a wildcard.
        /// </summary>
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void DoesNotMatchOnBlankExpectedFingerprint(string blank)
        {
            Assert.False(HostKeyFingerprint.Matches(Actual, new[] { blank }));
        }

        /// <summary>
        /// A server offering no fingerprint must not match a blank expectation.
        /// </summary>
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void DoesNotMatchOnBlankActualFingerprint(string blank)
        {
            Assert.False(HostKeyFingerprint.Matches(blank, new[] { blank }));
        }
    }
}
