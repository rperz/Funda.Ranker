using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funda.Ranker.Models;

namespace Funda.Ranker.Services
{
    public class SimpleRealtorRanker : IRanker<Realtor, int>
    {
        private readonly IObjectService _objectService;

        public SimpleRealtorRanker(IObjectService objectService)
        {
            _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
        }

        public async Task<IOrderedEnumerable<KeyValuePair<Realtor, int>>> GetRankedList(ListingType listingType, params string[] searchTerms)
        {
            var objectsForSale = await _objectService.GetObjects(listingType, searchTerms);
            var numberOfObjectsPerRealtor = objectsForSale.GroupBy(o => o.Realtor, o => o).ToDictionary(o => o.Key, o => o.Count());

            // first order by the actual number of items and then by the name of the realtor for reproducable results.
            var orderedList = numberOfObjectsPerRealtor.OrderByDescending(rank => rank.Value).ThenBy(rank => rank.Key.Name);
            return orderedList;
        }
    }
}