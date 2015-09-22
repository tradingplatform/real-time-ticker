using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Infusion.Trading.MarketData.Models
{
    public static class MarketDataSettings
    {
        public static string TickerWatchServerAddress { get; private set; }
        public static string RealTimeUpdateServerAddress { get; private set; }
        public static string SnapshotDataServerAddress { get; private set; }
        public static string FleckServerHost { get; set; }
        public static int FleckServerPort { get; set; }

        public static IList<string> StartupTickers { get; private set; }
        public static int ServerRefreshMillis { get; set; }

        public static string OverlayNotifyHost { get; set; }


        static MarketDataSettings()
        {
            TickerWatchServerAddress = ConfigurationManager.AppSettings["tickerWatchServer"];
            RealTimeUpdateServerAddress = ConfigurationManager.AppSettings["pubSubServer"];
            SnapshotDataServerAddress = ConfigurationManager.AppSettings["snapShotServer"];
            OverlayNotifyHost = ConfigurationManager.AppSettings["overlayhost"] ?? "http://10.1.103.60/";

            FleckServerHost = ConfigurationManager.AppSettings["fleckHost"] ?? "localhost";

            var fleckPort = 0;
            int.TryParse(ConfigurationManager.AppSettings["fleckPort"], out fleckPort);
            FleckServerPort = fleckPort;

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
