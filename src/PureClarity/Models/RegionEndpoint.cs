namespace PureClarity.Models
{
    internal class RegionEndpoint
    {
        public string APIEndpoint;
        public string SFTPEndpoint;

        /// <summary>Acceptable SHA256 host key fingerprints for <see cref="SFTPEndpoint"/>. More than one may be listed to span a host key rotation.</summary>
        public string[] SFTPHostKeyFingerprints;

        public RegionEndpoint(string aPIEndpoint, string sFTPEndpoint, params string[] sFTPHostKeyFingerprints)
        {
            this.APIEndpoint = aPIEndpoint;
            this.SFTPEndpoint = sFTPEndpoint;
            this.SFTPHostKeyFingerprints = sFTPHostKeyFingerprints ?? new string[0];
        }
    }
}