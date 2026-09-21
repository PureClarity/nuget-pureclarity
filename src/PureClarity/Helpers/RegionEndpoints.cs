using System;
using PureClarity.Models;

namespace PureClarity.Helpers
{
    internal static class RegionEndpoints
    {
        // Index 0 previously held a plaintext HTTP development endpoint and is intentionally unavailable.
        // Remaining indices must stay stable: callers pass the region as an int.
        private static readonly RegionEndpoint[] regionEndpoints = {
            null,
            new RegionEndpoint("https://api-eu-w-1.pureclarity.net", "sftp-eu-w-1.pureclarity.net", "SHA256:Uo5tnulN0tWtlcSH5dgeCNOuPl4yZ2XmTkILosOO/wY"),
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
            if (region < 0 || region >= regionEndpoints.Length || regionEndpoints[region] == null)
            {
                throw new ArgumentOutOfRangeException(nameof(region), region, "Unknown PureClarity region.");
            }

            return regionEndpoints[region];
        }
    }
}