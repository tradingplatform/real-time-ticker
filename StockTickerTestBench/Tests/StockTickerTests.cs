using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Infusion.Trading.MarketData.Client;
using Infusion.Trading.MarketData.Models;
using Infusion.Trading.MarketData.Models.Util;
using NUnit.Framework;

namespace StockTickerTestBench.Tests
{
    [TestFixture]
    public class StockTickerTests
    {
        [Test]
        public void ShouldPushQuoteMessages()
        {
            var zmqClient = new ZmqClient();
            var client = new ChumbyClient();

            for (var i = 0; i < 50; i++)
            {
                var quotes = zmqClient.GetQuoteInfo(MarketDataSettings.StartupTickers.ToArray());
                client.SendTickerUpdates(quotes.ToArray());
                Thread.Sleep(2000);
            }
        }
    }
}
