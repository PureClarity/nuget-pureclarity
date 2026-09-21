using PureClarity.Helpers;
using Xunit;

namespace PureClarity_Test
{
    public class RegionEndpointsTest
    {
        private const string EuFingerprint = "SHA256:Uo5tnulN0tWtlcSH5dgeCNOuPl4yZ2XmTkILosOO/wY";

        /// <summary>
        /// Every region is served by the EU infrastructure, including regions that never resolved
        /// and the retired plaintext development region 0.
        /// </summary>
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(4)]
        [InlineData(14)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        public void ResolvesEveryRegionToEu(int region)
        {
            var endpoint = RegionEndpoints.GetRegionEndpoints(region);

            Assert.Equal("https://api-eu-w-1.pureclarity.net", endpoint.APIEndpoint);
            Assert.Equal("sftp-eu-w-1.pureclarity.net", endpoint.SFTPEndpoint);
        }

        [Fact]
        public void NeverUsesPlaintextHttp()
        {
            Assert.StartsWith("https://", RegionEndpoints.GetRegionEndpoints(0).APIEndpoint);
        }

        /// <summary>
        /// Publishing refuses to connect without a pinned host key, so losing this would take publishing offline.
        /// </summary>
        [Fact]
        public void PinsEuHostKey()
        {
            Assert.Contains(EuFingerprint, RegionEndpoints.GetRegionEndpoints(1).SFTPHostKeyFingerprints);
        }
    }
}
