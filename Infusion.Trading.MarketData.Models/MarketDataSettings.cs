using System;
using System.Collections.Generic;
using System.Configuration;

namespace Infusion.Trading.MarketData.Models
{
    public static class MarketDataSettings
    {
        public static string TickerWatchServerAddress { get; private set; }
        public static string RealTimeUpdateServerAddress { get; private set; }
        public static string SnapshotDataServerAddress { get; private set; }
        public static IList<string> StartupTickers { get; private set; }
        public static int ServerRefreshMillis { get; set; }

        static MarketDataSettings()
        {
            TickerWatchServerAddress = ConfigurationManager.AppSettings["tickerWatchServer"];
            RealTimeUpdateServerAddress = ConfigurationManager.AppSettings["pubSubServer"];
            SnapshotDataServerAddress = ConfigurationManager.AppSettings["snapShotServer"];
            
            var tickers = ConfigurationManager.AppSettings["startupTickers"] ?? string.Empty;
            var tickerList = tickers.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            StartupTickers = tickerList;

            var serverRefreshValue = ConfigurationManager.AppSettings["serverRefreshMillis"];
            var refreshInterval = 100;
            int.TryParse(serverRefreshValue, out refreshInterval);
            ServerRefreshMillis = refreshInterval;
        }
    }
}
