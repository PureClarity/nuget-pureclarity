using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PureClarity.Collections;
using PureClarity.Helpers;
using PureClarity.Models;
using PureClarity.Models.Response;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace PureClarity.Managers
{
    internal class PublishManager
    {
        private readonly string accessKey;
        private readonly string secretKey;
        private readonly IReadOnlyList<string> hostKeyFingerprints;
        private readonly string dateFormat = "yyyyMdHHmmss";
        private readonly string deltaEndpointSuffix = "/api/productdelta";

        public PublishManager(string accessKey, string secretKey, IReadOnlyList<string> hostKeyFingerprints = null)
        {
            this.accessKey = accessKey;
            this.secretKey = secretKey;
            this.hostKeyFingerprints = hostKeyFingerprints ?? new string[0];
        }

        public async Task<PublishFeedResult> PublishProductFeed(IEnumerable<Product> products, IEnumerable<AccountPrice> accountPrices)
        {
            try
            {
                var productFeed = ConversionManager.ProcessProductFeed(products, accountPrices);
                var feedJSON = JSONSerialization.SerializeToJSON(productFeed);
                var endpoint = RegionEndpoints.GetEndpoints();
                await UploadToSTFPAsync(feedJSON, endpoint.SFTPEndpoint);
                return new PublishFeedResult { Success = true, Token = "" };
            }
            catch (Exception e)
            {               
                return new PublishFeedResult { Success = false, Error = e.Message, StackTrace = e.StackTrace };
            }
        }

        public async Task<PublishDeltaResult> PublishProductDeltas(IEnumerable<Product> products, IEnumerable<DeletedProductSku> deletedProducts, IEnumerable<AccountPrice> accountPrices, IEnumerable<DeletedAccountPrice> deletedAccountPrices, string accessKey)
        {
            var deltas = new List<ProcessedProductDelta>();
            var publishDeltaResult = new PublishDeltaResult();
            var endpoint = RegionEndpoints.GetEndpoints();
            var fullEndpoint = $"{endpoint.APIEndpoint}{deltaEndpointSuffix}";

            try
            {
                deltas = ConversionManager.ProcessProductDeltas(products, deletedProducts, accountPrices, deletedAccountPrices, accessKey);
            }
            catch (Exception e)
            {
                return new PublishDeltaResult
                {
                    Success = false,
                    Errors = new List<PublishDeltaError> {
                     new PublishDeltaError {
                         Error = e.Message,
                         Skus = products.Count() > 0 ? products.Select(prod => prod.Sku) : null,
                         DeletedSkus = deletedProducts.Count() > 0 ? deletedProducts.Select(deletedProd => deletedProd.Sku) : null,
                         AccountPrices = accountPrices,
                         DeletedAccountPrices = deletedAccountPrices
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
                    publishDeltaResult.Errors.Add(new PublishDeltaError {
                         Error = e.Message,
                         Skus = products.Count() > 0 ? products.Select(prod => prod.Sku) : null,
                         DeletedSkus = deletedProducts.Count() > 0 ? deletedProducts.Select(deletedProd => deletedProd.Sku) : null,
                         AccountPrices = accountPrices,
                         DeletedAccountPrices = deletedAccountPrices
                         });
                }
            }

            publishDeltaResult.Success = publishDeltaResult.Errors.Count == 0;
            return publishDeltaResult;
        }

        public async Task<PublishFeedResult> PublishCategoryFeed(IEnumerable<Category> categories)
        {
            try
            {
                var categoryFeed = ConversionManager.ProcessCategories(categories);
                var feedJSON = JSONSerialization.SerializeToJSON(categoryFeed);
                var endpoint = RegionEndpoints.GetEndpoints();
                await UploadToSTFPAsync(feedJSON, endpoint.SFTPEndpoint);
                return new PublishFeedResult { Success = true, Token = "" };
            }
            catch (Exception e)
            {
                return new PublishFeedResult { Success = false, Error = e.Message };
            }
        }

        public async Task<PublishFeedResult> PublishBrandFeed(IEnumerable<Brand> brands)
        {
            try
            {
                var brandFeed = ConversionManager.ProcessBrands(brands);
                var feedJSON = JSONSerialization.SerializeToJSON(brandFeed);
                var endpoint = RegionEndpoints.GetEndpoints();
                await UploadToSTFPAsync(feedJSON, endpoint.SFTPEndpoint);
                return new PublishFeedResult { Success = true, Token = "" };
            }
            catch (Exception e)
            {
                return new PublishFeedResult { Success = false, Error = e.Message };
            }
        }

        public async Task<PublishFeedResult> PublishUserFeed(IEnumerable<User> users)
        {
            try
            {
                var userFeed = ConversionManager.ProcessUsers(users);
                var feedJSON = JSONSerialization.SerializeToJSON(userFeed);
                var endpoint = RegionEndpoints.GetEndpoints();
                await UploadToSTFPAsync(feedJSON, endpoint.SFTPEndpoint);
                return new PublishFeedResult { Success = true, Token = "" };
            }
            catch (Exception e)
            {
                return new PublishFeedResult { Success = false, Error = e.Message };
            }
        }

        private async Task UploadToSTFPAsync(string json, string endpoint)
        {
            var fingerprints = GetExpectedHostKeyFingerprints(endpoint);

            var connectionInfo = new ConnectionInfo(endpoint, 2222,
                                                   this.accessKey,
                                                   new[] { new PasswordAuthenticationMethod(this.accessKey, this.secretKey) });
            using (var client = new SftpClient(connectionInfo))
            {
                client.HostKeyReceived += (sender, e) => e.CanTrust = IsTrustedHostKey(e, fingerprints);

                await client.ConnectAsync(CancellationToken.None);

                using (MemoryStream jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    await client.UploadFileAsync(jsonStream, $"PureClarityFeed-{DateTime.UtcNow.ToString(dateFormat)}.json", CancellationToken.None);
                }

                client.Disconnect();
            }
        }

        private IReadOnlyList<string> GetExpectedHostKeyFingerprints(string endpoint)
        {
            var fingerprints = hostKeyFingerprints.Count > 0
                ? hostKeyFingerprints
                : RegionEndpoints.GetEndpoints().SFTPHostKeyFingerprints;

            if (fingerprints == null || fingerprints.Count == 0)
            {
                throw new InvalidOperationException(
                    $"No SFTP host key fingerprints are configured for {endpoint}, so the server's identity cannot be verified. " +
                    "Supply the expected SHA256 fingerprints via the FeedManager constructor.");
            }

            return fingerprints;
        }

        private static bool IsTrustedHostKey(HostKeyEventArgs e, IReadOnlyList<string> expectedFingerprints)
        {
            return HostKeyFingerprint.Matches(e.FingerPrintSHA256, expectedFingerprints);
        }
    }
}