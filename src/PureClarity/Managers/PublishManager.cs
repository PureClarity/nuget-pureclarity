using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using PureClarity.Collections;
using PureClarity.Helpers;
using PureClarity.Models;
using Renci.SshNet;
using Renci.SshNet.Async;

namespace PureClarity.Managers
{
    public class PublishManager
    {
        private readonly string accessKey;
        private readonly string secretKey;
        private readonly Region region;
        private readonly string dateFormat = "yyyyMdHHmmss";

        public PublishManager(string accessKey, string secretKey, Region region)
        {
            this.accessKey = accessKey;
            this.secretKey = secretKey;
            this.region = region;
        }

        public async Task<PublishFeedResult> PublishProductFeed(ProcessedProductFeed products)
        {
            try
            {
                var prods = JSONSerialization.SerializeToJSON(products);
                await UploadToSTFP(prods);
                return new PublishFeedResult { Success = true, Token = "" };
            }
            catch (Exception e)
            {
                return new PublishFeedResult { Success = false, Error = e.Message };
            }
        }

        private async Task UploadToSTFP(string json)
        {
            var connectionInfo = new ConnectionInfo("localhost", 2222,
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