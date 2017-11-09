using System.Collections.Generic;
using PureClarity.Models;

namespace PureClarity.Validators
{
    internal abstract class PCValidationBase
    {
        protected IDictionary<string, IEnumerable<string>> InvalidRecords;

        public PCValidationBase()
        {
            InvalidRecords = new Dictionary<string, IEnumerable<string>>();
        }

        public CollectionValidationResult GetValidationResult()
        {
            return new CollectionValidationResult
            {
                Success = InvalidRecords.Count == 0,
                InvalidRecords = InvalidRecords
            };
        }
    }
}