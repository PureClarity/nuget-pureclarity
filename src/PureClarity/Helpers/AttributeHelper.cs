using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;

namespace PureClarity.Helpers
{
    public class AttributeHelper
    {
        public static JArray GetJTokenAttributeValueArray(IEnumerable<string> attributeValues)
        {
            var encodedAttributeValues = attributeValues.Select((attr) =>
            {
                return WebUtility.HtmlEncode(attr);
            });

            return new JArray(encodedAttributeValues.ToArray());
        }
    }
}