using PureClarity.Models;

namespace PureClarity.Helpers
{
    public class RegionEndpoints
    {
        public static RegionEndpoint GetRegionEndpoints(Region region)
        {
            switch (region)
            {
                case Region.EU_West_1:
                    return new RegionEndpoint("api-eu-w-1.pureclarity.net", "sftp-eu-w-1 .pureclarity.net.");
                case Region.US_East_1:
                    break;
                default:
                    throw new System.Exception("Not a valid region selection");
            }
            return null;
        }
    }
}