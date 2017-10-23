using System.Collections.Generic;

namespace PureClarity.Models.Response
{
    public class QueryTokensResult : HttpResponse
    {
        public List<TokenStatus> TokenStatuses;

        public QueryTokensResult()
        {
            TokenStatuses = new List<TokenStatus>();
        }

    }
}