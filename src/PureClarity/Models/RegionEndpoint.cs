namespace PureClarity.Models
{
    internal class RegionEndpoint
    {
        public string APIEndpoint;
        public string SFTPEndpoint;

        public RegionEndpoint(string aPIEndpoint, string sFTPEndpoint)
        {
            this.APIEndpoint = aPIEndpoint;
            this.SFTPEndpoint = sFTPEndpoint;
        }
    }
}