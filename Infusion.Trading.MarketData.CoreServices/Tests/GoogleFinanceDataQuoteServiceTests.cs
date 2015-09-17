using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infusion.Trading.MarketData.CoreServices.Services;
using NUnit.Framework;

namespace Infusion.Trading.MarketData.CoreServices.tests
{
    [TestFixture]
    public class GoogleFinanceDataQuoteServiceTests
    {
        [Test]
        public void ShouldFetchQuotes()
        {
            var svc = new GoogleFinanceDataQuoteService();
            var quotes = svc.GetQuotes("MSFT", "GOOG", "AAPL");
            Assert.That(quotes, Is.Not.Null);
            Assert.That(quotes.Count, Is.Not.LessThanOrEqualTo(0));
        }
    }
}
