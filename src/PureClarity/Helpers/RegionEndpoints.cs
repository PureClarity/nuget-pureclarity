using PureClarity.Models;

namespace PureClarity.Helpers
{
    internal static class RegionEndpoints
    {
        // Only the EU infrastructure exists. The historical per-region hostnames are DNS aliases
        // that resolve to it.
        private static readonly RegionEndpoint euEndpoint = new RegionEndpoint(
            "https://api-eu-w-1.pureclarity.net",
            "sftp-eu-w-1.pureclarity.net",
            "SHA256:Uo5tnulN0tWtlcSH5dgeCNOuPl4yZ2XmTkILosOO/wY");

        public static RegionEndpoint GetEndpoints()
        {
            return euEndpoint;
        }
    }
}