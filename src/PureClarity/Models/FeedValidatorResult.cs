using System.Collections.Generic;

namespace PureClarity.Models
{
    public class FeedValidatorResult : PCResultBase
    {
        public ValidatorResult ProductValidationResult;
        public ValidatorResult CategoryValidationResult;
        public ValidatorResult BrandValidationResult;
        public ValidatorResult UserValidationResult;
    }
}