using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funda.Ranker.Models;

namespace Funda.Ranker.Services
{
    public class ObjectForSaleSErvice : IObjectForSaleService
    {
        private readonly FundaClient _client;

        public ObjectForSaleSErvice(FundaClient client)
        {
            _client = client;
        }
        public async Task<IEnumerable<ObjectForSale>> GetAllObjectsForSale(params string[] searchTerms)
        {
            var objects = new List<ObjectForSale>();
            int pageNumber = 1;
            int pageSize = 25;
            var objectsFromWebservice = await _client.GetObjectsForSale(pageNumber, pageSize, 1, searchTerms).ConfigureAwait(false);
            while (objectsFromWebservice.Count() > 0)
            {
                objects.AddRange(objectsFromWebservice);
                pageNumber++;
                objectsFromWebservice = await _client.GetObjectsForSale(pageNumber, pageSize, 1, searchTerms).ConfigureAwait(false);
            }

            return objects;
        }
    }
}
