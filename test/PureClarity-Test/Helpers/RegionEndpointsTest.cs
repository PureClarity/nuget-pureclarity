using PureClarity.Helpers;
using Xunit;

namespace PureClarity_Test
{
    public class RegionEndpointsTest
    {
        private const string EuFingerprint = "SHA256:Uo5tnulN0tWtlcSH5dgeCNOuPl4yZ2XmTkILosOO/wY";

        [Fact]
        public void ResolvesToEu()
        {
            var endpoint = RegionEndpoints.GetEndpoints();

            Assert.Equal("https://api-eu-w-1.pureclarity.net", endpoint.APIEndpoint);
            Assert.Equal("sftp-eu-w-1.pureclarity.net", endpoint.SFTPEndpoint);
        }

        [Fact]
        public void NeverUsesPlaintextHttp()
        {
            Assert.StartsWith("https://", RegionEndpoints.GetEndpoints().APIEndpoint);
        }

        /// <summary>
        /// Publishing refuses to connect without a pinned host key, so losing this would take publishing offline.
        /// </summary>
        [Fact]
        public void PinsEuHostKey()
        {
            Assert.Contains(EuFingerprint, RegionEndpoints.GetEndpoints().SFTPHostKeyFingerprints);
        }
    }
}
