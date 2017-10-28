using EasyList.Proto.Core.Retailers;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EasyList.Proto.Retailers.Carrefour
{
    public class RetailerLocator : IRetailerLocator
    {
        private readonly Retailer retailer;

        public RetailerLocator(Retailer retailer)
        {
            this.retailer = retailer;
        }

        public async Task<IEnumerable<IStore>> FindStoresAsync(string query)
        {
            using (var client = new HttpClient())
            {
                var @string = await client.GetStringAsync("https://courses-en-ligne.carrefour.fr/search-stores/" + query.Trim());

                JArray storeArray = (JArray)JObject.Parse(@string)["data"];

                return from store in storeArray
                       where (int)store["storeRef"] > 0
                       select new Store
                       {
                           Retailer = retailer,
                           Id = (int)store["storeRef"],
                           Name = (string)store["name"],
                           Address = (string)store["address"],
                           City = (string)store["city"],
                           ZipCode = (string)store["zipCode"],
                           Latitude = (double)store["lat"],
                           Longitude = (double)store["lng"]
                       };
            }
        }
    }
}
