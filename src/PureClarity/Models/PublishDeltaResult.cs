using System.Collections.Generic;

namespace PureClarity.Models
{
    public class PublishDeltaResult : PCResultBase
    {
        public List<string> Tokens;
        public List<PublishDeltaError> Errors;

        public PublishDeltaResult(){
            Tokens = new List<string>();
            Errors = new List<PublishDeltaError>();
        }
    }
}