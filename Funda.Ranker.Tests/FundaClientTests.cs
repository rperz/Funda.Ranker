using System;
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
            var result = await sut.GetObjectsForSale().ConfigureAwait(false);

            Assert.AreEqual(25, result.Objects.Count);
        }

        [Test]
        public async Task GetObjectsForSaleShouldThrowExceptionWhenMoreThan100RequestsWithinAMinute()
        {
            var logger = new Mock<ILogger>();
            var sut = new FundaClient(logger.Object);
            var tasks = new Task[150];
            for (int i = 0; i < 150; i++)
            {
                await sut.GetObjectsForSale();
            }
        }
    }
}
