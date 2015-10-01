using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Infusion.Trading.MarketData.Models;
using Infusion.Trading.MarketData.Models.Serialization;
using Infusion.Trading.MarketData.Models.Util;

namespace Infusion.Trading.MarketData.Client
{
    public class ChumbyClient
    {
        private readonly ConcurrentQueue<Quote> updateQueue = new ConcurrentQueue<Quote>();
        private readonly AutoResetEvent processQueueHandle = new AutoResetEvent(false);

        public ChumbyClient()
        {
            Task.Factory.StartNew(() =>
            {
                using (var client = new WebClient())
                {
                    while (true)
                    {
                        if (updateQueue.IsEmpty)
                        {
                            processQueueHandle.WaitOne();
                            continue;
                        }

                        Quote quote;
                        if (!updateQueue.TryDequeue(out quote))
                        {
                            processQueueHandle.WaitOne();
                            continue;
                        }

                        var message =
                            $"[{quote.Symbol}] {quote.Ask.ToStringOrDefault()}  Change: {quote.Change.ToStringOrDefault()}   Low: {quote.DaysLow.ToStringOrDefault()}  High: {quote.DaysHigh.ToStringOrDefault()}";

                        var queryUri = $"{MarketDataSettings.OverlayNotifyHost}bridge?cmd=tickerevent&message={message}&title=";
                        var responseStream = client.OpenRead(queryUri);
                        responseStream?.Close();
                        responseStream?.Dispose();
                        
                        Thread.Sleep(1000);
                    }
                }
            });
        }

        public void SendTickerUpdates(params Quote[] quotes)
        {
            foreach (var quote in quotes)
            {
                updateQueue.Enqueue(quote);
            }

            processQueueHandle.Set();
        }
    }
}
