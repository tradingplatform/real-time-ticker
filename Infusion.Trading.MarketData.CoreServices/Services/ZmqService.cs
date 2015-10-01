using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infusion.Trading.MarketData.CoreServices.Contracts;
using Infusion.Trading.MarketData.Models;
using Infusion.Trading.MarketData.Models.Util;
using NetMQ;
using Newtonsoft.Json;

namespace Infusion.Trading.MarketData.CoreServices.Services
{
    public class ZmqService : IDisposable, IZmqService
    {
        private string Id = Guid.NewGuid().ToString();
        private readonly IQuoteService primitiveQuoteService;
        private static readonly List<string> subscribedTickerList = new List<string>();
        private readonly NetMQContext socketFactory;
        private readonly int refreshInterval;

        public event Action<IList<Quote>> HandleTickerInfoPublish;

        public ZmqService(YahooFinancialDataService primitiveQuoteService)
        {
            this.primitiveQuoteService = primitiveQuoteService;
            
            subscribedTickerList.AddRange(MarketDataSettings.StartupTickers);
            refreshInterval = MarketDataSettings.ServerRefreshMillis;

            socketFactory = NetMQContext.Create();
        }

        public void Start()
        {
            Task.Factory.StartNew(() =>
            {
                ListenForSnapshotRequest();
                ListenForSubscriptions();
                PublishTickerInfo();
            });
        }

        public void SubscribeTicker(string ticker)
        {
            lock (subscribedTickerList)
            {
                if (!subscribedTickerList.Contains(ticker))
                {
                    subscribedTickerList.Add(ticker);
                }
            }
        }

        private void ListenForSnapshotRequest()
        {
            Task.Factory.StartNew((() =>
            {
                using (var server = socketFactory.CreateResponseSocket())
                {
                    server.Bind(MarketDataSettings.SnapshotDataServerAddress);

                    while (true)
                    {
                        var message = server.ReceiveFrameString();

                        if (string.IsNullOrEmpty(message))
                        {
                            Thread.Sleep(100);
                            continue;
                        }

                        if (message.ToLower() == "quit") break;

                        Console.WriteLine($"Snapshot requested for ticker(s) {message}");

                        var securityIds = message.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                        var response = primitiveQuoteService.GetQuotes(securityIds);

                        var responseString = JsonConvert.SerializeObject(response);
                        server.SendFrame(responseString);
                        Thread.Sleep(100);
                    }
                }
            }));
        }

        private void ListenForSubscriptions()
        {
            Task.Factory.StartNew(() =>
            {
                using (var server = socketFactory.CreateResponseSocket())
                {
                    server.Bind(MarketDataSettings.TickerWatchServerAddress);

                    while (true)
                    {
                        var message = server.ReceiveFrameString();

                        if (string.IsNullOrEmpty(message))
                        {
                            Thread.Sleep(100);
                            continue;
                        }

                        if (message.ToLower() == "quit") break;

                        // processing the request
                        Console.WriteLine($"Subscription requested for {message}");
                        SubscribeTicker(message);

                        server.SendFrame(message);
                        Thread.Sleep(100);
                    }
                }
            });
            Console.WriteLine("Initialized listener for ticker subscriptions");
        }

        private void PublishTickerInfo()
        {
            using (var pubSocket = socketFactory.CreatePublisherSocket())
            {
                pubSocket.Options.SendHighWatermark = 1000;
                pubSocket.Bind(MarketDataSettings.RealTimeUpdateServerAddress);

                while (true)
                {
                    // processing the request
                    Thread.Sleep(100);

                    IList<Quote> tickerInfos = null;

                    lock (subscribedTickerList)
                    {
                        tickerInfos = primitiveQuoteService.GetQuotes(subscribedTickerList.ToArray());
                    }

                    if (!tickerInfos.Any())
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    HandleTickerInfoPublish?.Invoke(tickerInfos);

                    var asOf = DateTime.Now;
                    foreach (var item in tickerInfos)
                    {
                        var responseString = JsonConvert.SerializeObject(item);
                        pubSocket.SendMoreFrame(item.Symbol).SendFrame(responseString);
                        Console.WriteLine($"Published update for {item.Symbol} at {asOf.ToString("hh:mm:ss")}");
                    }

                    Thread.Sleep(refreshInterval);
                }
            }
        }

        ~ZmqService()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                socketFactory?.Dispose();
            }
        }
    }
}
