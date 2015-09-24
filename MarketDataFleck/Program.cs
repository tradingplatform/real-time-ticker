using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fleck;
using Infusion.Trading.MarketData.Client;
using Infusion.Trading.MarketData.CoreServices.Services;
using Infusion.Trading.MarketData.Models;
using Infusion.Trading.MarketData.Models.Serialization;
using Newtonsoft.Json;

namespace MarketDataFleck
{
    /// <summary>
    /// A quick-and-dirty pub/sub mediator (aka proxy). This is the bottleneck server where all subscriptions are set, 
    /// and where zmq updates are processed and pushed to the websocket clients. Honestly, making a more elaborate design
    /// would have eaten more time than worth for a video-overlay solution that this server is designed to service.
    /// 
    /// Expect to notice design + code smells!
    /// </summary>
    class Program
    {
        private static readonly List<IWebSocketConnection> allSockets = new List<IWebSocketConnection>();
        private static bool isConnected = false;

        private static void Main(string[] args)
        {
            var zmqClient = new ZmqClient();
            zmqClient.HandleSubscriberUpdate += ZmqClientOnHandleSubscriberUpdate;
            zmqClient.Start(MarketDataSettings.StartupTickers.ToArray());

            var fleckHost = string.Empty;
            var fleckAddr =
                Dns.GetHostAddresses(fleckHost).First(a => a.AddressFamily == AddressFamily.InterNetwork).ToString();

            var fleckServer = new WebSocketServer($"ws://{fleckAddr}:{MarketDataSettings.FleckServerPort}");

            fleckServer.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine($"Opened fleck server socket at {DateTime.Now.ToString("hh:mm:ss")}");
                    allSockets.Add(socket);
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine($"Closed flecker server socket at {DateTime.Now.ToString("hh:mm:ss")}");
                    allSockets.Remove(socket);
                };

                socket.OnMessage = rawMessage =>
                {
                    // parse the list of tickers we expect to get in the message.
                    var tickerList = rawMessage.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                    zmqClient.UpdateSubscriptions(tickerList);
                    
                    // not going to send a response back yet... only when we get an update from zmq server.
                    var s = string.Format("{0}", tickerList[0]);
                };

                isConnected = true;
            });

            // a bit of a code smell, but Fleck will just die unless we go into a spin mode.
            while (true)
            {
                Thread.Sleep(100);
            }
        }

        private static void ZmqClientOnHandleSubscriberUpdate(Quote quote)
        {
            if (!isConnected) return;

            // we got an update from zmq - deserialize & push to all ws clients
            var quoteString = JsonConvert.SerializeObject(quote);
            allSockets.ToList().ForEach(s => s.Send(quoteString));
        }
    }
}
