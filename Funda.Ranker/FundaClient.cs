using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Funda.Ranker
{
    public class FundaClient
    {
        private readonly ILogger _logger;
        private readonly HttpClient _client;
        private readonly string _baseUrl;
        private const int MaxRetries = 10;
        public FundaClient(ILogger logger)
        {
            _logger = logger;
            _baseUrl = "http://partnerapi.funda.nl/feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f/?type=koop&zo=/amsterdam/tuin/&page=1&pagesize=25";
            _client = new HttpClient();
        }

        public async Task<FundaResultDTO> GetObjectsForSale(int tryNumber = 1)
        {
            if (tryNumber > MaxRetries)
            {
                throw new RequestLimitExceededException();
            }

            var response = await _client.GetAsync(_baseUrl);

            if (IsRequestLimitExceeded(response.StatusCode, response.ReasonPhrase))
            {
                _logger.Info($"Request limit exceeded. Waiting for a minute before trying again. Try number {tryNumber} of {MaxRetries}");
                Thread.Sleep(60000);
                await this.GetObjectsForSale(tryNumber++).ConfigureAwait(false);
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.StatusCode + ": " + response.ReasonPhrase);
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FundaResultDTO>(content);
        }

        private bool IsRequestLimitExceeded(HttpStatusCode statusCode, string reason)
        {
            return statusCode == HttpStatusCode.Unauthorized && reason.Equals("Request limit exceeded")
        }
    }
}
