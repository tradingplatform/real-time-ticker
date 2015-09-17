using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Infusion.Trading.MarketData.CoreServices.Contracts;
using Newtonsoft.Json;
using System.Composition;
using Infusion.Trading.MarketData.Models;

namespace Infusion.Trading.MarketData.CoreServices.Services
{
    public class GoogleFinanceDataQuoteService : IQuoteService
    {
        private const string EXCHANGE = "NASDAQ";

        // TODO: Don't use Google Finance - it's no longer officially supported and it doesn't appear to have all of the data we need anyway.
        // See example response file in the Solution Items folder: oogleFinanceResponse-Example.json
        //http://www.google.com/finance/info?infotype=infoquoteall&q=NASDAQ:AAPL

        private readonly Uri baseUrl = new Uri("http://www.google.com/finance/");

        public IList<Quote> GetQuotes(params string[] securityIds)
        {
            return GetQuotesInternal(securityIds).Result;
        }

        private async Task<IList<Quote>> GetQuotesInternal(params string[] securityIds)
        {
            if (securityIds == null || !securityIds.Any()) return new List<Quote>();

            var result = new List<Quote>();
            var securityIdsAsString = string.Join(",", securityIds);
            
            var queryUri = $"info?infotype=infoquoteall&q={EXCHANGE}:{securityIdsAsString}";
            using (var client = new HttpClient {BaseAddress = baseUrl})
            {
                var response = await client.GetStringAsync(queryUri).ConfigureAwait(false);
                var deserializedRawList = (dynamic)JsonConvert.DeserializeObject(response.Substring(4));
                var asOf = DateTime.Now;

                // JK: we should probably redo this using Automapper or something similar, 
                // to improve testability of the field-to-field mappings
                foreach (var quote in deserializedRawList)
                {
                    result.Add(new Quote
                    {
                        SecurityId = quote.t,           // t = ticker
                        Exchange = quote.e,             // e = exchange
                        Price = (decimal)quote.l,       // l = list(?) price
                        LastChange = (decimal)quote.c,  // c = change
                        DayHigh = (decimal)quote.hi,    // hi = day high
                        DayLow = (decimal)quote.lo,     // lo = day low
                        AsOf = asOf                     // how fresh?
                    });
                }
            }
            return result;
        } 
    }
}