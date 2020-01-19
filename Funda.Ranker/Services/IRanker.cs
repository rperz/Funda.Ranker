using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funda.Ranker.Models;

namespace Funda.Ranker.Services
{
    public interface IRanker<T1, T2>
    {
        Task<IOrderedEnumerable<KeyValuePair<T1, T2>>> GetRankedList(ListingType listingType, params string[] searchTerms);
    }
}
