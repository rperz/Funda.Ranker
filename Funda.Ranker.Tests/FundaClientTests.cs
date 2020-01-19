using System;
using System.Linq;
using System.Threading.Tasks;
using Funda.Ranker.Communication;
using Funda.Ranker.Logging;
using Funda.Ranker.Models;
using Moq;
using NUnit.Framework;

namespace Funda.Ranker.Tests
{
    [TestFixture]
    public class FundaClientTests
    {
        [Test]
        public async Task GetObjectsForSaleShouldReturnNumberOfItemsRequested()
        {
            var logger = new Mock<ILogger>();
            var sut = new FundaClient(logger.Object, new FundaConfiguration() { MaxRetries = 20, SleepTimeAfterExceedingRequestLimit = 5000, BaseUrl = "http://partnerapi.funda.nl/feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f/?type={0}{1}&page={2}&pagesize={3}" });
            var result = await sut.GetObjects(ListingType.Sale, 1, 25, 1,  "amsterdam", "tuin").ConfigureAwait(false);

            Assert.AreEqual(25, result.Result.Count());
        }

        [Test]
        public async Task GetObjectsForSaleShouldFinishAllRequestsThrowExceptionWhenMoreThan100RequestsWithinAMinute()
        {
            var logger = new Mock<ILogger>();
            var sut = new FundaClient(logger.Object, new FundaConfiguration() { MaxRetries = 20, SleepTimeAfterExceedingRequestLimit = 5000, BaseUrl = "http://partnerapi.funda.nl/feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f/?type={0}{1}&page={2}&pagesize={3}" });
            var tasks = new Task[150];
            for (int i = 0; i < 150; i++)
            {
                await sut.GetObjects(ListingType.Sale, 1, 25, 1, "tuin", "amsterdam").ConfigureAwait(false);
            }
        }
    }
}
