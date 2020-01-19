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
        private readonly int _maxRetries = 10;
        private readonly int _sleepTime;

        public FundaClient(ILogger logger, FundaConfiguration configuration)
        {
            _logger = logger;
            _maxRetries = configuration.MaxRetries;
            _sleepTime = configuration.SleepTimeAfterExceedingRequestLimit;
            _baseUrl = configuration.BaseUrl;
            _client = new HttpClient();
        }

        public async Task<PagedResult<IEnumerable<ObjectForSale>>> GetObjects(ListingType listingType, int pageNumber, int pageSize, int tryNumber = 1, params string[] searchTerms)
        {
            if (tryNumber > _maxRetries)
            {
                throw new RequestLimitExceededException();
            }

            var queryString = "";
            if (searchTerms != null && searchTerms.Length > 0)
            {
                var searchTermsAsQueryString = string.Join('/', searchTerms);
                queryString = $"&zo=/{searchTermsAsQueryString}/";
            }

            var requestUrl = string.Format(_baseUrl, ListingTypeToTypeString(listingType), queryString, pageNumber, pageSize);
            var response = await _client.GetAsync(requestUrl);

            if (IsRequestLimitExceeded(response.StatusCode, response.ReasonPhrase))
            {
                _logger.Info($"Request limit exceeded. Waiting for {_sleepTime / 1000} seconds before trying again. Try number {tryNumber} of {_maxRetries}");
                Thread.Sleep(5000);
                return await this.GetObjects(listingType, pageNumber, pageNumber, ++tryNumber, searchTerms).ConfigureAwait(false);
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.StatusCode + ": " + response.ReasonPhrase);
            }

            var content = await response.Content.ReadAsStringAsync();
            var fundaObjects = JsonConvert.DeserializeObject<FundaResultDTO>(content);
            return new PagedResult<IEnumerable<ObjectForSale>>()
            {
                CurrentPage = fundaObjects.Paging.HuidigePagina,
                NumberOfPages = fundaObjects.Paging.AantalPaginas,
                Result = fundaObjects.Objects.Select(o =>
                    new ObjectForSale(o.Id, o.Adres, new Realtor(o.MakelaarId, o.MakelaarNaam)))
            };
        }

        private bool IsRequestLimitExceeded(HttpStatusCode statusCode, string reason)
        {
            return statusCode == HttpStatusCode.Unauthorized && reason.Equals("Request limit exceeded");
        }

        private string ListingTypeToTypeString(ListingType type)
        {
            switch (type)
            {
                case ListingType.Rent:
                    return "huur";
                case ListingType.Sale:
                    return "koop";
                default: 
                    throw new ArgumentException($"Unknown listingtype {type}");
            }
        }
    }
}
