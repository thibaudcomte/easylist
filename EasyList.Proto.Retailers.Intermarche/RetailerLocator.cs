using EasyList.Proto.Core.Retailers;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EasyList.Proto.Retailers.Intermarche
{
    public class RetailerLocator : IRetailerLocator
    {
        private object m_lock = new object();
        private IEnumerable<Store> m_storesCache;

        private Retailer m_retailer;

        public RetailerLocator(Retailer retailer)
        {
            m_retailer = retailer;
        }

        public async Task<Store> GetStoreFromIdAsync(int id)
        {
            await EnsureCacheStoresAsync();
            return m_storesCache.First(s => s.Id == id);
        }

        private async Task<IEnumerable<Store>> GetStoresAsync()
        {
            using (var client = new HttpClient())
            {
                var @string = await client.GetStringAsync("https://drive.intermarche.com/GetPdvs");

                JArray storeArray = JArray.Parse(@string);

                return from store in storeArray
                       where (int)store["dri"] == 1
                       select new Store
                       {
                           Retailer = m_retailer,
                           Id = (int)store["id"],
                           Idt = store.Contains("idt") ? (int)store["idt"] : -1,
                           Name = (string)store["nom"],
                           Address = (string)store["adr"],
                           City = (string)store["nos"],
                           ZipCode = (string)store["cp"],
                           Latitude = (int)store["lat"],
                           Longitude = (int)store["lon"],
                           Urlh = (string)store["urlh"]
                       };
            }
        }

        private async Task EnsureCacheStoresAsync()
        {
            lock (m_lock)
            {
                if (m_storesCache != null) return;
            }

            m_storesCache = await GetStoresAsync();
        }

        public async Task<IEnumerable<IStore>> FindStoresAsync(string search)
        {
            await EnsureCacheStoresAsync();

            search = search.ToLower();

            var stores = from store in m_storesCache
                         where store.ZipCode.StartsWith(search) || store.City.ToLower().Contains(search)
                         select store;

            return stores;
        }
    }
}
