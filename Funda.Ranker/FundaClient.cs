using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Funda.Ranker.Models;
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
            _baseUrl = "http://partnerapi.funda.nl/feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f/?type=koop{0}&page={1}&pagesize={2}";
            _client = new HttpClient();
        }

        public async Task<IEnumerable<ObjectForSale>> GetObjectsForSale(int pageNumber, int pageSize, int tryNumber = 1, params string[] searchTerms)
        {
            if (tryNumber > MaxRetries)
            {
                throw new RequestLimitExceededException();
            }

            var queryString = "";
            if (searchTerms != null && searchTerms.Length > 0)
            {
                var searchTermsAsQueryString = string.Join('/', searchTerms);
                queryString = $"&zo=/{searchTermsAsQueryString}/";
            }

            var requestUrl = string.Format(_baseUrl, queryString, pageNumber, pageSize);
            var response = await _client.GetAsync(requestUrl);

            if (IsRequestLimitExceeded(response.StatusCode, response.ReasonPhrase))
            {
                _logger.Info($"Request limit exceeded. Waiting for 5 seconds before trying again. Try number {tryNumber} of {MaxRetries}");
                Thread.Sleep(5000);
                return await this.GetObjectsForSale(pageNumber, pageNumber, ++tryNumber, searchTerms).ConfigureAwait(false);
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.StatusCode + ": " + response.ReasonPhrase);
            }

            var content = await response.Content.ReadAsStringAsync();
            var fundaObjects = JsonConvert.DeserializeObject<FundaResultDTO>(content);
            return fundaObjects.Objects.Select(o =>
                new ObjectForSale(o.Id, o.Adres, new Realtor(o.MakelaarId, o.MakelaarNaam)));
        }

        private bool IsRequestLimitExceeded(HttpStatusCode statusCode, string reason)
        {
            return statusCode == HttpStatusCode.Unauthorized && reason.Equals("Request limit exceeded");
        }
    }
}
