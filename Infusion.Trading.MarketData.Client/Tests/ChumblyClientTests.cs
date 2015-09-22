using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Infusion.Trading.MarketData.Models;
using NUnit.Framework;

namespace Infusion.Trading.MarketData.Client.Tests
{
    [TestFixture]
    public class ChumblyClientTests
    {
        [Test]
        public void ShouldPostScrollingMessage()
        {
            var message = "<b>testing</b> and more testing";
            message = Uri.EscapeUriString(message);
            var queryUri = $"{MarketDataSettings.OverlayNotifyHost}bridge?cmd=tickerevent&message={message}&title=Unit";

            using (var client = new WebClient())
            {
                //client.GetAsync(queryUri).ConfigureAwait(false);
                var strm = client.OpenRead(queryUri);
                strm?.Close();
                strm?.Dispose();
            }
        }
    }
}
