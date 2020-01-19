using System;
using System.Linq;
using System.Threading.Tasks;
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
            var sut = new FundaClient(logger.Object);
            var result = await sut.GetObjects(1, 25, 1, "tuin", "amsterdam").ConfigureAwait(false);

            Assert.AreEqual(25, result.Result.Count());
        }

        [Test]
        public async Task GetObjectsForSaleShouldFinishAllRequestsThrowExceptionWhenMoreThan100RequestsWithinAMinute()
        {
            var logger = new Mock<ILogger>();
            var sut = new FundaClient(logger.Object);
            var tasks = new Task[150];
            for (int i = 0; i < 150; i++)
            {
                await sut.GetObjects(1, 25, 1, "tuin", "amsterdam").ConfigureAwait(false);
            }
        }
    }
}
