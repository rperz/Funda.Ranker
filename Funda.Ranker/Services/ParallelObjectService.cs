using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funda.Ranker.Communication;
using Funda.Ranker.Models;

namespace Funda.Ranker.Services
{
    public class ParallelObjectService : IObjectService
    {
        private readonly IFundaClient _client;
        private readonly ServiceConfiguration _configuration;
        private readonly int _pageSize;

        public ParallelObjectService(IFundaClient client, ServiceConfiguration configuration)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _pageSize = configuration.PageSize;
        }
        public async Task<IEnumerable<ObjectForSale>> GetObjects(ListingType type, params string[] searchTerms)
        {
            var objects = new ConcurrentBag<ObjectForSale>();
            var pageNumber = 1;
            var pageSize = _configuration.PageSize;
            var objectsFromWebservice = await _client.GetObjects(type, pageNumber, pageSize, 1, searchTerms).ConfigureAwait(false);
            var tasks = new Task[objectsFromWebservice.NumberOfPages-1];
            for (var i = 0; i < objectsFromWebservice.NumberOfPages - 1; i++)
            {
                tasks[i] = AddPageToConcurrentBag(type, i+1, objects, searchTerms);
            }

            await Task.WhenAll(tasks);

            return objects;
        }

        private async Task AddPageToConcurrentBag(ListingType type, int pageNumber, ConcurrentBag<ObjectForSale> objects, params string[] searchTerms)
        {
            var result = await _client.GetObjects(type, pageNumber, _pageSize, 1, searchTerms).ConfigureAwait(false);
            foreach(var currentObject in result.Result)
            {
                objects.Add(currentObject);
            }
        }
    }
}
