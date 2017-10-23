using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PureClarity.Helpers;
using PureClarity.Models;
using PureClarity.Models.Response;

namespace PureClarity.Managers
{
    public class QueryTokenManager
    {
        private readonly string _accessKey;
        private readonly int _region;
        private readonly string deltaStatusEndpointSuffix = "/api/productdeltastatus";
        private readonly string _fullEndpoint;

        public QueryTokenManager(string accessKey, int region)
        {
            _accessKey = accessKey ?? throw new System.ArgumentNullException(nameof(accessKey));
            _region = region;
            var endpoint = RegionEndpoints.GetRegionEndpoints(region);
            _fullEndpoint = $"{endpoint.APIEndpoint}{deltaStatusEndpointSuffix}";
        }

        public QueryTokensResult QueryTokens(IEnumerable<string> tokens)
        {
            try
            {
                return CallTokenStatus(tokens).Result;
            }
            catch (Exception e)
            {
                return new QueryTokensResult { Error = e.Message };
            }
        }

        public async Task<QueryTokensResult> QueryTokensAsync(IEnumerable<string> tokens)
        {
            try
            {
                return await CallTokenStatus(tokens);
            }
            catch (Exception e)
            {
                return new QueryTokensResult { Error = e.Message };
            }
        }

        private async Task<QueryTokensResult> CallTokenStatus(IEnumerable<string> tokens)
        {
            var queryResult = new QueryTokensResult();
            var tokenFeed = new TokenFeed { AppKey = _accessKey, Tokens = tokens.ToArray() };
            var json = JSONSerialization.SerializeToJSON(tokenFeed);
            var resonse = await HttpCalls.Post<JArray>(json, _fullEndpoint);

            if (resonse != null)
            {
                foreach (var jsonObject in resonse)
                {
                    var token = JsonConvert.DeserializeObject<TokenStatus>(jsonObject.ToString());
                    queryResult.TokenStatuses.Add(token);
                }
            }

            return queryResult;
        }
    }
}