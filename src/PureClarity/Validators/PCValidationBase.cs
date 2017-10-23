using System.Collections.Generic;
using PureClarity.Models;

namespace PureClarity.Validators
{
    public abstract class PCValidationBase
    {
        protected IDictionary<string, IEnumerable<string>> InvalidRecords;

        public PCValidationBase()
        {
            InvalidRecords = new Dictionary<string, IEnumerable<string>>();
        }

        public ValidationResult GetValidationResult()
        {
            return new ValidationResult
            {
                Success = InvalidRecords.Count == 0,
                InvalidRecords = InvalidRecords
            };
        }
    }
}