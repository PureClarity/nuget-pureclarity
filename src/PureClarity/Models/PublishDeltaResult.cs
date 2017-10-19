using System.Collections.Generic;

namespace PureClarity.Models
{
    public class PublishDeltaResult : PCResultBase
    {
        public IEnumerable<string> Tokens;
        public IEnumerable<PublishDeltaError> Errors;
    }
}