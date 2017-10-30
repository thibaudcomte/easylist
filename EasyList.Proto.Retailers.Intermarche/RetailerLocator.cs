using EasyList.Proto.Core.Retailers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EasyList.Proto.Retailers.Intermarche
{
    public class RetailerLocator : IRetailerLocator
    {
        private Retailer _Retailer;

        public RetailerLocator(Retailer retailer)
        {
            _Retailer = retailer;
        }

        public async Task<IEnumerable<IStore>> FindStoresAsync(string search)
        {
            using (var client = new HttpClient())
            {
                var @string = await client.GetStringAsync("https://drive.intermarche.com/GetPdvs");

                JArray storeArray = JArray.Parse(@string);

                return from store in storeArray
                       where (int)store["dri"] == 1
                       let city = (string)store["nos"]
                       where city.StartsWith(search, StringComparison.CurrentCultureIgnoreCase)
                       select new Store
                       {
                           Retailer = _Retailer,
                           Id = (int)store["id"],
                           Idt = store.Contains("idt") ? (int)store["idt"] : -1,
                           Name = (string)store["nom"],
                           Address = (string)store["adr"],
                           City = city,
                           ZipCode = (string)store["cp"],
                           Latitude = (double)store["lat"],
                           Longitude = (double)store["lon"],
                           Urlh = (string)store["urlh"]
                       };
            }
        }
    }
}
