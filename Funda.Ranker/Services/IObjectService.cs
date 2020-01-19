using System.Collections.Generic;
using System.Threading.Tasks;
using Funda.Ranker.Models;

namespace Funda.Ranker.Services
{
    public interface IObjectService
    {
        Task<IEnumerable<ObjectForSale>> GetObjects(ListingType listingType, params string[] searchTerms);
    }
}