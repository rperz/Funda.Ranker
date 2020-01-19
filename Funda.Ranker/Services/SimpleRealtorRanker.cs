﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funda.Ranker.Services;

namespace Funda.Ranker
{
    public class SimpleRealtorRanker : IRanker<Realtor, int>
    {
        private readonly ObjectForSaleSErvice _objectForSaleService;

        public SimpleRealtorRanker(ObjectForSaleSErvice objectForSaleService)
        {
            _objectForSaleService = objectForSaleService;
        }

        public async Task<IOrderedEnumerable<KeyValuePair<Realtor, int>>> GetRankedList(params string[] searchTerms)
        {
            var objectsForSale = await _objectForSaleService.GetAllObjectsForSale(searchTerms);
            var numberOfObjectsPerRealtor = objectsForSale.GroupBy(o => o.Realtor, o => o).ToDictionary(o => o.Key, o => o.Count());

            // first order by the actual number of items and then by the name of the realtor for reproducable results.
            var orderedList = numberOfObjectsPerRealtor.OrderByDescending(rank => rank.Value).ThenBy(rank => rank.Key.Name);
            return orderedList;
        }
    }
}