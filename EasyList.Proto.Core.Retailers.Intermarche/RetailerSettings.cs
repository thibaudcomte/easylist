using EasyList.Proto.Core.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;

namespace EasyList.Proto.Core.Retailers.Intermarche
{
    [DataContract]
    public class PersistentRetailerSettings
    {
        [DataMember]
        public int[] UserStoreIds { get; set; }
    }

    public class RetailerSettings : IRetailerSettings
    {
        private readonly List<Store> _UserStores;
        IEnumerable<IStore> IRetailerSettings.UserStores => _UserStores;

        private readonly RetailerLocator _RetailerLocator;
        private readonly IStorageReaderWriter _StorageReaderWriter;

        public RetailerSettings(RetailerLocator retailerLocator, IStorageReaderWriter storageReaderWriter)
        {
            _UserStores = new List<Store>();
            _RetailerLocator = retailerLocator;
            _StorageReaderWriter = storageReaderWriter;
        }

        public async Task LoadAsync()
        {
            PersistentRetailerSettings persistentSettings = await _StorageReaderWriter.ReadAsync<PersistentRetailerSettings>();

            _UserStores.Clear();

            if (persistentSettings == null || persistentSettings.UserStoreIds == null)
            {
                return;
            }

            foreach (var id in persistentSettings.UserStoreIds)
            {
                var store = await _RetailerLocator.GetStoreFromIdAsync(id);
                if (store != null)
                {
                    _UserStores.Add(store);
                }
            }
        }

        public async Task SaveAsync()
        {
            PersistentRetailerSettings persistentSettings = new PersistentRetailerSettings
            {
                UserStoreIds = _UserStores.Select(store => store.Id).ToArray()
            };

            await _StorageReaderWriter.WriteAsync(persistentSettings);
        }

        public void AddUserStore(IStore store)
        {
            if (store is Store)
            {
                _UserStores.Add(store as Store);
            }
        }

        public void RemoveUserStore(IStore store)
        {
            if (store is Store)
            {
                _UserStores.Remove(store as Store);
            }
        }

        public void ClearUserStores()
        {
            _UserStores.Clear();
        }
    }
}
