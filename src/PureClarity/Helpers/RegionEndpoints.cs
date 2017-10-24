using PureClarity.Models;

namespace PureClarity.Helpers
{
    internal class RegionEndpoints
    {

        private static RegionEndpoint[] regionEndpoints = {
            new RegionEndpoint("http://api.pureclarity.dev:1337", "localhost"),
            new RegionEndpoint("https://api-eu-w-1.pureclarity.net", "sftp-eu-w-1.pureclarity.net"),
            new RegionEndpoint("https://api-eu-w-2.pureclarity.net", "sftp-eu-w-2.pureclarity.net"),
            new RegionEndpoint("https://api-eu-c-1.pureclarity.net", "sftp-eu-c-1.pureclarity.net"),
            new RegionEndpoint("https://api-us-e-1.pureclarity.net", "sftp-us-e-1.pureclarity.net"),
            new RegionEndpoint("https://api-us-e-2.pureclarity.net", "sftp-us-e-2.pureclarity.net"),
            new RegionEndpoint("https://api-us-w-1.pureclarity.net", "sftp-us-w-1.pureclarity.net"),
            new RegionEndpoint("https://api-us-w-2.pureclarity.net", "sftp-us-w-2.pureclarity.net"),
            new RegionEndpoint("https://api-ap-s-1.pureclarity.net", "sftp-ap-s-1.pureclarity.net"),
            new RegionEndpoint("https://api-ap-ne-1.pureclarity.net", "sftp-ap-ne-1.pureclarity.net"),
            new RegionEndpoint("https://api-ap-ne-2.pureclarity.net", "sftp-ap-ne-2.pureclarity.net"),
            new RegionEndpoint("https://api-ap-se-1.pureclarity.net", "sftp-ap-se-1.pureclarity.net"),
            new RegionEndpoint("https://api-ap-se-2.pureclarity.net", "sftp-ap-se-2.pureclarity.net"),
            new RegionEndpoint("https://api-ca-c-1.pureclarity.net", "sftp-ca-c-1.pureclarity.net"),
            new RegionEndpoint("https://api-sa-e-1.pureclarity.net", "sftp-sa-e-1.pureclarity.net")
        };

        public static RegionEndpoint GetRegionEndpoints(int region)
        {
            return regionEndpoints[region];
        }
    }
}