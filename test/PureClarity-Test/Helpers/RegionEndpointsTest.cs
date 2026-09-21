using System;
using PureClarity.Helpers;
using Xunit;

namespace PureClarity_Test
{
    public class RegionEndpointsTest
    {
        [Fact]
        public void ReturnsEndpointsForKnownRegion()
        {
            var endpoint = RegionEndpoints.GetRegionEndpoints(1);

            Assert.Equal("https://api-eu-w-1.pureclarity.net", endpoint.APIEndpoint);
            Assert.Equal("sftp-eu-w-1.pureclarity.net", endpoint.SFTPEndpoint);
        }

        /// <summary>
        /// Publishing refuses to connect without a pinned host key, so losing this would take the region offline.
        /// </summary>
        [Fact]
        public void PinsHostKeyForEuWest1()
        {
            var endpoint = RegionEndpoints.GetRegionEndpoints(1);

            Assert.Contains("SHA256:Uo5tnulN0tWtlcSH5dgeCNOuPl4yZ2XmTkILosOO/wY", endpoint.SFTPHostKeyFingerprints);
        }

        /// <summary>
        /// Region 0 was a plaintext HTTP development endpoint and must no longer resolve.
        /// </summary>
        [Fact]
        public void RejectsRetiredDevelopmentRegion()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => RegionEndpoints.GetRegionEndpoints(0));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(15)]
        [InlineData(int.MaxValue)]
        public void RejectsOutOfRangeRegion(int region)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => RegionEndpoints.GetRegionEndpoints(region));
        }

        [Fact]
        public void AllKnownRegionsUseHttps()
        {
            for (var region = 1; region <= 14; region++)
            {
                Assert.StartsWith("https://", RegionEndpoints.GetRegionEndpoints(region).APIEndpoint);
            }
        }
    }
}
