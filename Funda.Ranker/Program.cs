using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Funda.Ranker.IoC;
using Funda.Ranker.Services;
using Microsoft.Extensions.Configuration;
using SimpleInjector;

namespace Funda.Ranker
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var container = new Container();
            container.RegisterParallelRanker(25, 5000, "http://partnerapi.funda.nl/feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f/?type={0}{1}&page={2}&pagesize={3}", 20);

            var realtorRanker = container.GetInstance<IRanker<Realtor, int>>();

            Console.WriteLine("Loading Top 10 Realtors (objects for sale)....");
            var bestRealtorsWithObjectsForSale = await realtorRanker.GetRankedList(ListingType.Sale,"amsterdam");

            Console.WriteLine("Top 10 Realtors (objects for sale)");
            WriteResultToConsole(bestRealtorsWithObjectsForSale);

            Console.WriteLine("Loading Top 10 Realtors (objects with garden for sale)....");
            var bestRealtorsWithObjectsForSaleWithGarden = await realtorRanker.GetRankedList(ListingType.Sale,"amsterdam", "tuin");
            Console.WriteLine("Top 10 Realtors (objects with garden for sale)");
            WriteResultToConsole(bestRealtorsWithObjectsForSaleWithGarden);

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }


        private static void WriteResultToConsole(IOrderedEnumerable<KeyValuePair<Realtor, int>> result)
        {
            Console.WriteLine("Id".PadRight(10) + "\tName".PadRight(75) + "\tObjects for sale");
            foreach (var realtor in result.Take(10))
            {
                Console.WriteLine($"{realtor.Key.Id.ToString().PadRight(10)}\t{realtor.Key.Name.PadRight(75)}\t{realtor.Value}");
            }
        }
    }
}
