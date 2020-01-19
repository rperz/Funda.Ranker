using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funda.Ranker.Services;

namespace Funda.Ranker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var realtorRanker = new SimpleRealtorRanker(new ObjectForSaleSErvice(new FundaClient(new ConsoleLogger())));

            Console.WriteLine("Loading Top 10 Realtors (objects for sale)....");
            var bestRealtorsWithObjectsForSale = await realtorRanker.GetRankedList("amsterdam");

            Console.WriteLine("Top 10 Realtors (objects for sale)");
            WriteResultToConsole(bestRealtorsWithObjectsForSale);

            Console.WriteLine("Loading Top 10 Realtors (objects with garden for sale)....");
            var bestRealtorsWithObjectsForSaleWithGarden = await realtorRanker.GetRankedList("amsterdam", "tuin");
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
