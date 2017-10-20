using System.Collections.Generic;

namespace PureClarity.Models
{
    public class PublishResult : PCResultBase
    {
        public PublishFeedResult PublishProductFeedResult;
        public PublishFeedResult PublishCategoryFeedResult;
        public PublishFeedResult PublishBrandFeedResult;
        public PublishFeedResult PublishUserFeedResult;
    }
}