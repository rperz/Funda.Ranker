using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funda.Ranker.Models;

namespace Funda.Ranker.Services
{
    public class ObjectService : IObjectService
    {
        private readonly FundaClient _client;
        private readonly ServiceConfiguration _configuration;

        public ObjectService(FundaClient client, ServiceConfiguration configuration)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task<IEnumerable<ObjectForSale>> GetObjects(ListingType type, params string[] searchTerms)
        {
            var objects = new List<ObjectForSale>();
            var pageNumber = 1;
            var pageSize = _configuration.PageSize;
            var objectsFromWebservice = await _client.GetObjects(type, pageNumber, pageSize, 1, searchTerms).ConfigureAwait(false);
            while (objectsFromWebservice.Result.Count() > 0)
            {
                objects.AddRange(objectsFromWebservice.Result);
                pageNumber++;
                objectsFromWebservice = await _client.GetObjects(type, pageNumber, pageSize, 1, searchTerms).ConfigureAwait(false);
            }

            return objects;
        }
    }
}
