using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace EasyList.Proto.Core.Retailers.Intermarche
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
                var array = JsonArray.Parse(@string);

                var drives = new List<Store>(array.Count);

                foreach (var item in array)
                {
                    var @object = item.GetObject();
                    try
                    {
                        if (@object.GetNamedNumber("dri") == 1)
                        {
                            drives.Add(new Store
                            {
                                Retailer = m_retailer,
                                Id = (int)@object.GetNamedNumber("id"),
                                Idt = @object.GetNamedValue("idt").ValueType == JsonValueType.Null ?
                                -1 : int.Parse(@object.GetNamedString("idt")),
                                Name = @object.GetNamedString("nom"),
                                Address = @object.GetNamedString("adr"),
                                City = @object.GetNamedString("nos"),
                                ZipCode = @object.GetNamedString("cp"),
                                Latitude = (int)@object.GetNamedNumber("lat"),
                                Longitude = (int)@object.GetNamedNumber("lon"),
                                Urlh = @object.GetNamedString("urlh")
                            });
                        }
                    }
                    catch
                    {
                        Debug.WriteLine($"Drive '{@object}' is not well formed. Ignore it.");
                    }
                }

                return drives;
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
