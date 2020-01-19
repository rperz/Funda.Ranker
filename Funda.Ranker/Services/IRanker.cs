using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funda.Ranker
{
    public interface IRanker<T1, T2>
    {
        Task<IOrderedEnumerable<KeyValuePair<T1, T2>>> GetRankedList(ListingType listingType, params string[] searchTerms);
    }
}
