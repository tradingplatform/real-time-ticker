using System.Collections.Generic;
using System.Threading.Tasks;
using Infusion.Trading.MarketData.Models;

namespace Infusion.Trading.MarketData.CoreServices.Contracts
{
    public interface IQuoteService
    {
        IList<Quote> GetQuotes(params string[] securityIds);
    }
}
