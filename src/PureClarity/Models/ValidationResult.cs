using System.Collections.Generic;

namespace PureClarity.Models
{
    public class ValidationResult : PCResultBase
    {
        public CollectionValidationResult ProductValidationResult;
        public CollectionValidationResult AccountPriceValidationResult;
        public CollectionValidationResult CategoryValidationResult;
        public CollectionValidationResult BrandValidationResult;
        public CollectionValidationResult UserValidationResult;
    }
}