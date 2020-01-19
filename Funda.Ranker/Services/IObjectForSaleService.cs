using System.Collections.Generic;
using System.Threading.Tasks;
using Funda.Ranker.Models;

namespace Funda.Ranker.Services
{
    public interface IObjectForSaleService
    {
        Task<IEnumerable<ObjectForSale>> GetAllObjectsForSale(params string[] searchTerms);
    }
}