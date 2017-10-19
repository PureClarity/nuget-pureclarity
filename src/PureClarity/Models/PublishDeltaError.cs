using System.Collections.Generic;

namespace PureClarity.Models
{
    public class PublishDeltaError
    {
        public string Error;
        public IEnumerable<string> Skus;
    }
}