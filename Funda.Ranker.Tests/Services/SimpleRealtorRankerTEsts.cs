using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funda.Ranker.Models;
using Funda.Ranker.Services;
using Moq;
using NUnit.Framework;

namespace Funda.Ranker.Tests.Services
{
    [TestFixture]
    public class SimpleRealtorRankerTests
    {
        [Test]
        public async Task GetRankingShouldCallIntoObjectForSaleServiceWithCorrectParameters()
        {
            var objectForSaleService = new Mock<IObjectService>();
            var sut = new SimpleRealtorRanker(objectForSaleService.Object);

            await sut.GetRankedList(ListingType.Sale,"Test", "Tweede");

            objectForSaleService.Verify(s=>s.GetObjects(ListingType.Sale,"Test", "Tweede"));
        }

        [Test]
        public async Task GetRankingShouldReturnRankedList()
        {
            var object1Guid = Guid.NewGuid();
            var object2Guid = Guid.NewGuid();
            var object3Guid = Guid.NewGuid();
            var objectForSaleService = new Mock<IObjectService>();
            var objectsForSale = new List<ObjectForSale>();
            objectsForSale.AddRange(GetRandomObjectForSale(1, "Robin").Take(10));
            objectsForSale.AddRange(GetRandomObjectForSale(2, "Kristel").Take(15));
            objectsForSale.AddRange(GetRandomObjectForSale(3, "Yeti").Take(12));

            objectForSaleService.Setup(s => s.GetObjects(ListingType.Sale, "Test", "Tweede")).ReturnsAsync(objectsForSale);

            var sut = new SimpleRealtorRanker(objectForSaleService.Object);

            var rank = await sut.GetRankedList(ListingType.Sale,"Test", "Tweede");

            var immutableRank = rank.ToImmutableList();
            Assert.AreEqual(immutableRank[0].Key, new Realtor(2, "Kristel"));
            Assert.AreEqual(immutableRank[1].Key, new Realtor(3, "Yeti"));
            Assert.AreEqual(immutableRank[2].Key, new Realtor(1, "Robin"));
            Assert.AreEqual(immutableRank[0].Value, 15);
            Assert.AreEqual(immutableRank[1].Value, 12);
            Assert.AreEqual(immutableRank[2].Value, 10);

        }

        private IEnumerable<ObjectForSale> GetRandomObjectForSale(int realtorId, string realtorName)
        {
            while (true)
            {
                byte[] convertToName = new byte[15];
                new Random().NextBytes(convertToName);
                var randomAddress = convertToName.ToString();
                yield return new ObjectForSale(Guid.NewGuid(), randomAddress, new Realtor(realtorId, realtorName));
            }
        }
    }
}
