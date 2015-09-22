using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Infusion.Trading.MarketData.Client;
using Infusion.Trading.MarketData.Models;

namespace StockTickerTestBench
{
    class Program
    {
        static void Main(string[] args)
        {
            var zmqClient = new ZmqClient();
            var client = new ChumblyClient();

            while (true)
            {
                var quotes = zmqClient.GetQuoteInfo(MarketDataSettings.StartupTickers.ToArray());
                client.SendTickerUpdates(quotes.ToArray());
                Thread.Sleep(4000);
            }
        }
    }
}
