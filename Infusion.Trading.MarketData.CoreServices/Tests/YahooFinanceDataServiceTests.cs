using Infusion.Trading.MarketData.CoreServices.Services;
using NUnit.Framework;

namespace Infusion.Trading.MarketData.CoreServices.tests
{
    [TestFixture]
    public class YahooFinanceDataServiceTests
    {
        [Test]
        public void ShouldFetchQuotes()
        {
            var svc = new YahooFinancialDataService();
            var quotes = svc.GetQuotes("MSFT", "GOOG", "AAPL");
            Assert.That(quotes, Is.Not.Null);
            Assert.That(quotes.Count, Is.Not.LessThanOrEqualTo(0));
        }
    }
}