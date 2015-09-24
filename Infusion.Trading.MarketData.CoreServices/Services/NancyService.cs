using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Infusion.Trading.MarketData.CoreServices.Contracts;
using Infusion.Trading.MarketData.Models;
using Nancy;
using Nancy.Hosting.Self;
using Nancy.ModelBinding;

namespace Infusion.Trading.MarketData.CoreServices.Services
{
    public class QuotesModule : NancyModule
    {
        private readonly IQuoteService quoteService;
        private readonly IZmqService publishingService;

        public QuotesModule(IQuoteService quoteService, IZmqService publishingService) 
            : base("/")
        {
            if (quoteService == null) throw new ArgumentNullException(nameof(quoteService));
            if (publishingService == null) throw new ArgumentNullException(nameof(publishingService));
            this.quoteService = quoteService;
            this.publishingService = publishingService;

            Get["/quote/{id}"] = parameter =>
            {
                var securityIds = (string)parameter.id;
                var quotes = quoteService.GetQuotes(securityIds);

                var responseModel = quotes.Count == 0 ? new Quote() : quotes[0];

                return Response.AsJson(responseModel);
            };

            Get["/subscribe/{id}"] = parameter =>
            {
                var securityId = (string) parameter.id;

                publishingService.SubscribeTicker(securityId);

                return Response.AsText($"{securityId} has been subscribed.");
            };
        }
    }

    public class NancyService
    {
        public void Start()
        {
            var hostname = Dns.GetHostName();
            var uriList = Dns.GetHostAddresses(hostname);

            if (uriList.All(u => u.AddressFamily != AddressFamily.InterNetwork)) return;

            var ipAddress = uriList.First(u => u.AddressFamily == AddressFamily.InterNetwork);

            var config = new HostConfiguration
            {
                UrlReservations = new UrlReservations {CreateAutomatically = true},
                RewriteLocalhost = false
            };

            using (var host = new NancyHost(config, new Uri($"http://{ipAddress.ToString()}:{MarketDataSettings.RestServicePort}")))
            {
                host.Start();
                for (;;) { }
            }
        }
    }
}
