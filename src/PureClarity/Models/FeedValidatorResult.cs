using System.Collections.Generic;

namespace PureClarity.Models
{
    public class FeedValidationResult : PCResultBase
    {
        public ValidationResult ProductValidationResult;
        public ValidationResult CategoryValidationResult;
        public ValidationResult BrandValidationResult;
        public ValidationResult UserValidationResult;
    }
}