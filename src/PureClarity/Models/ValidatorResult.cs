using System.Collections.Generic;

namespace PureClarity.Models
{
    public class ValidatorResult : PCResultBase
    {
        public IDictionary<string, IEnumerable<string>> InvalidRecords;
    }
}