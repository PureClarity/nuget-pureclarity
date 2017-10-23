using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PureClarity.Collections;
using PureClarity.Helpers;
using PureClarity.Models;
using PureClarity.Models.Response;
using Renci.SshNet;
using Renci.SshNet.Async;

namespace PureClarity.Managers
{
    public class PublishManager
    {
        private readonly string accessKey;
        private readonly string secretKey;
        private readonly int region;
        private readonly string dateFormat = "yyyyMdHHmmss";
        private readonly string deltaEndpointSuffix = "/api/productdelta";

        public PublishManager(string accessKey, string secretKey, int region)
        {
            this.accessKey = accessKey;
            this.secretKey = secretKey;
            this.region = region;
        }

        public async Task<PublishFeedResult> PublishProductFeed(IEnumerable<Product> products)
        {
            try
            {
                var feed = ConversionManager.ProcessProductFeed(products);
                var prods = JSONSerialization.SerializeToJSON(feed);
                var endpoint = RegionEndpoints.GetRegionEndpoints(region);
                await UploadToSTFP(prods, endpoint.SFTPEndpoint);
                return new PublishFeedResult { Success = true, Token = "" };
            }
            catch (Exception e)
            {
                return new PublishFeedResult { Success = false, Error = e.Message };
            }
        }

        public async Task<PublishDeltaResult> PublishProductDeltas(IEnumerable<Product> products, string accessKey)
        {
            var deltas = new List<ProcessedProductDelta>();
            var publishDeltaResult = new PublishDeltaResult();
            var endpoint = RegionEndpoints.GetRegionEndpoints(region);
            var fullEndpoint = $"{endpoint.APIEndpoint}{deltaEndpointSuffix}";

            try
            {
                deltas = ConversionManager.ProcessProductDeltas(products, accessKey);
            }
            catch (Exception e)
            {
                return new PublishDeltaResult
                {
                    Success = false,
                    Errors = new List<PublishDeltaError> {
                     new PublishDeltaError {
                         Error = e.Message,
                         Skus = products.Select((prod)=> prod.Sku)
                         }
                      }
                };
            }

            foreach (var delta in deltas)
            {
                try
                {
                    var json = JSONSerialization.SerializeToJSON(delta);
                    var deltaResult = await HttpCalls.Post<DeltaResult>(json, fullEndpoint);
                    publishDeltaResult.Tokens.Add(deltaResult.Token);
                }
                catch (Exception e)
                {
                    publishDeltaResult.Errors.Add(new PublishDeltaError { Error = e.Message, Skus = delta.Products.Select((prod) => prod.Sku) });
                }
            }

            publishDeltaResult.Success = publishDeltaResult.Errors.Count == 0;
            return publishDeltaResult;
        }

        private async Task UploadToSTFP(string json, string endpoint)
        {
            var connectionInfo = new ConnectionInfo(endpoint, 2222,
                                                   this.accessKey,
                                                   new[] { new PasswordAuthenticationMethod(this.accessKey, this.secretKey) });
            using (var client = new SftpClient(connectionInfo))
            {
                client.Connect();

                using (MemoryStream jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    await client.UploadAsync(jsonStream, $"PureClarityFeed-{DateTime.UtcNow.ToString(dateFormat)}.json");
                }
            }
        }
    }
}