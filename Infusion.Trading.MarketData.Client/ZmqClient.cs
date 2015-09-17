using System;
using System.Collections.Generic;
using System.Linq;
using Infusion.Trading.MarketData.Models;
using NetMQ;
using NetMQ.Sockets;

namespace Infusion.Trading.MarketData.Client
{
    public class ZmqClient : IDisposable
    {
        private readonly List<string> tickerList = new List<string>();
        private readonly NetMQContext context;
        private readonly SubscriberSocket subSocket;

        public ZmqClient()
        {
            context = NetMQContext.Create();

            subSocket = context.CreateSubscriberSocket();
            subSocket.Options.ReceiveHighWatermark = 1000;
            subSocket.Connect(MarketDataSettings.RealTimeUpdateServerAddress);
        }

        public string GetQuoteInfo(params string[] tickerListToQuote)
        {
            if (tickerListToQuote == null || !tickerListToQuote.Any()) return string.Empty;

            using (var client = context.CreateRequestSocket())
            {
                client.Connect(MarketDataSettings.SnapshotDataServerAddress);
                var joinedList = string.Join(",", tickerListToQuote);

                client.SendFrame(joinedList);
                var response = client.ReceiveFrameString();
                return response;
            }
        }

        public void Start(params string[] tickerListToAdd)
        {
            UpdateSubscriptions(tickerListToAdd);
            ListenForUpdates();
        }

        public void UpdateSubscriptions(params string[] tickerListToAdd)
        {
            if (tickerListToAdd == null || !tickerListToAdd.Any()) return;

            using (var client = context.CreateRequestSocket())
            {
                tickerList.AddRange(tickerListToAdd);

                client.Connect(MarketDataSettings.TickerWatchServerAddress);

                foreach (var ticker in tickerListToAdd)
                {
                    client.SendFrame(ticker);
                    var response = client.ReceiveFrameString();
                }
            }
        }
        
        public void RemoveSubscription(params string[] tickerListToRemove)
        {
            if (tickerListToRemove == null) return;

            // should probably rethink this one... each socket should have its own data. might be hitting 
            // typical thread contention problems here. but not to worry about at the moment (ca. 2015/09/17)
            foreach (var ticker in tickerListToRemove)
            {
                subSocket.Unsubscribe(ticker);
                tickerList.Remove(ticker);
            }
        }

        public void ClearSubscriptions()
        {
            // should probably rethink this one... each socket should have its own data. might be hitting 
            // typical thread contention problems here. but not to worry about at the moment (ca. 2015/09/17)
            foreach (var ticker in tickerList)
            {
                subSocket.Unsubscribe(ticker);
            }

            tickerList.Clear();
        }

        private void ListenForUpdates()
        {
            foreach (var ticker in tickerList)
            {
                subSocket.Subscribe(ticker);
            }

            Console.WriteLine("Subscriber socket connecting...");
            while (true)
            {
                string messageTopicReceived = subSocket.ReceiveFrameString();
                string messageReceived = subSocket.ReceiveFrameString();
                Console.WriteLine(messageReceived);
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        ~ZmqClient()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                subSocket?.Dispose();
                context?.Dispose();
            }
        }
    }
}