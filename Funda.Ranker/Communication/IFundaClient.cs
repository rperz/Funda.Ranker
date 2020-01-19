using System.Collections.Generic;
using System.Threading.Tasks;
using Funda.Ranker.Models;

namespace Funda.Ranker.Communication
{
    public interface IFundaClient
    {
        Task<PagedResult<IEnumerable<ObjectForSale>>> GetObjects(ListingType listingType, int pageNumber, int pageSize, int tryNumber = 1, params string[] searchTerms);
    }
}