using System.Collections.Generic;

namespace PureClarity.Models
{
    public class CollectionValidationResult : PCResultBase
    {
        public IDictionary<string, IEnumerable<string>> InvalidRecords;
    }
}