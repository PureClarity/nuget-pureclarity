using System.Collections.Generic;

namespace PureClarity.Models
{
    public class ValidationResult : PCResultBase
    {
        public IDictionary<string, IEnumerable<string>> InvalidRecords;
    }
}