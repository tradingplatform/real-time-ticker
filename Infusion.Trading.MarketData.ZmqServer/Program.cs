using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using Infusion.Trading.MarketData.CoreServices;
using Infusion.Trading.MarketData.CoreServices.Services;

namespace Infusion.Trading.MarketData.ZmqServer
{
    class Program
    {
        private static void Main(string[] args)
        {
            // NOTE: ensure that the app.config entries as denoted by MarketDataSettings class
            // are properly set (check the Models project for the src code).
            using (var svc = new ZmqService())
            {
                svc.Start();
            }
        }
    }
}
