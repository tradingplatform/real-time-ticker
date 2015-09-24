using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infusion.Trading.MarketData.CoreServices.Contracts;

namespace Infusion.Trading.MarketData.CoreServices.Services
{
    public class MarketDataService
    {
        private readonly IZmqService zmqService;
        private readonly FleckService fleckService;
        private readonly NancyService nancyService;

        public MarketDataService(IZmqService zmqService, FleckService fleckService, NancyService nancyService)
        {
            if (zmqService == null) throw new ArgumentNullException(nameof(zmqService));
            if (fleckService == null) throw new ArgumentNullException(nameof(fleckService));
            if (nancyService == null) throw new ArgumentNullException(nameof(nancyService));

            this.zmqService = zmqService;
            this.fleckService = fleckService;
            this.nancyService = nancyService;
        }

        public void Start()
        {
            zmqService.Start();
            fleckService.Start();
            nancyService.Start();
        }
    }
}
