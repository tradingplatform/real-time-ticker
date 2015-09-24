using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Fleck;
using Infusion.Trading.MarketData.CoreServices.Contracts;
using Infusion.Trading.MarketData.Models;
using Newtonsoft.Json;

namespace Infusion.Trading.MarketData.CoreServices.Services
{
    public class FleckService
    {
        private readonly IZmqService publishingService;

        public FleckService(IZmqService publishingService)
        {
            if (publishingService == null) throw new ArgumentNullException(nameof(publishingService));
            this.publishingService = publishingService;
        }

        public void Start()
        {
            Task.Factory.StartNew(() =>
            {
                var fleckHost = string.Empty;
                var fleckAddr =
                    Dns.GetHostAddresses(fleckHost).First(a => a.AddressFamily == AddressFamily.InterNetwork).ToString();

                var fleckServer = new WebSocketServer($"ws://{fleckAddr}:{MarketDataSettings.FleckServerPort}");
                var allSockets = new List<IWebSocketConnection>();
                var isConnected = false;

                fleckServer.Start(socket =>
                {
                    socket.OnOpen = () =>
                    {
                        Console.WriteLine($"Opened fleck server socket at {DateTime.Now.ToString("hh:mm:ss")}");
                        allSockets.Add(socket);
                        isConnected = true;
                    };
                    socket.OnClose = () =>
                    {
                        Console.WriteLine($"Closed flecker server socket at {DateTime.Now.ToString("hh:mm:ss")}");
                        allSockets.Remove(socket);
                    };

                    socket.OnMessage = rawMessage =>
                    {
                        // any websocket msg from client is ignored - we get our inputs from REST api
                    };

                    isConnected = true;
                });

                publishingService.HandleTickerInfoPublish += listOfQuotes =>
                {
                    if (!isConnected || listOfQuotes == null) return;

                    // push the list of quotes down on the websockets
                    foreach (var quote in listOfQuotes)
                    {
                        var quoteString = JsonConvert.SerializeObject(quote);
                        allSockets.ToList().ForEach(s => s.Send(quoteString));
                    }
                };
            });
        }
    }
}
