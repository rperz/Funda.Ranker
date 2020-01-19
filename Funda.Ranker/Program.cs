using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Funda.Ranker.IoC;
using Funda.Ranker.Models;
using Funda.Ranker.Services;
using Microsoft.Extensions.Configuration;
using SimpleInjector;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;

namespace Funda.Ranker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            if (!int.TryParse(config["pageSize"], out var pageSize))
            {
                Console.WriteLine("Pagesize is not a correct number");
                Console.WriteLine("Press any key to exit..");
                Console.ReadLine();
                return;
            }

            if (!int.TryParse(config["sleepTimeOnRequestLimitExceededInMilliSeconds"], out var sleepTime))
            {
                Console.WriteLine("sleepTimeOnRequestLimitExceededInMilliSeconds is not a correct integer");
                Console.WriteLine("Press any key to exit..");
                Console.ReadLine();
                return;
            }

            if (!int.TryParse(config["maxRetries"], out var maxRetries))
            {
                Console.WriteLine("maxRetries is not a correct integer");
                Console.WriteLine("Press any key to exit..");
                Console.ReadLine();
                return;
            }

            var baseUrl = config["baseUrl"];

            try
            {
                var container = new Container();
                container.RegisterParallelRanker(pageSize, sleepTime, baseUrl, maxRetries);

                var realtorRanker = container.GetInstance<IRanker<Realtor, int>>();

                Console.WriteLine("Loading Top 10 Realtors (objects for sale)....");
                var bestRealtorsWithObjectsForSale = await realtorRanker.GetRankedList(ListingType.Sale, "amsterdam");

                Console.WriteLine("Top 10 Realtors (objects for sale)");
                WriteResultToConsole(bestRealtorsWithObjectsForSale);

                Console.WriteLine("Loading Top 10 Realtors (objects with garden for sale)....");
                var bestRealtorsWithObjectsForSaleWithGarden =
                    await realtorRanker.GetRankedList(ListingType.Sale, "amsterdam", "tuin");
                Console.WriteLine("Top 10 Realtors (objects with garden for sale)");
                WriteResultToConsole(bestRealtorsWithObjectsForSaleWithGarden);

                Console.WriteLine("Press any key to exit");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Unhandled Exception occured:");
                Console.WriteLine(e.Message);
                Console.WriteLine("Press any key to exit..");
                Console.ReadLine();
            }
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
