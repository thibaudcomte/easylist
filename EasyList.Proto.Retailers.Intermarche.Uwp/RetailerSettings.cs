using EasyList.Proto.Core.Storage;
using EasyList.Proto.Core.Uwp.Storage.LocalStorage;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace EasyList.Proto.Retailers.Intermarche.Uwp
{
    [DataContract]
    public class PersistentRetailerSettings
    {
        [DataMember]
        public int[] UserStoreIds { get; set; }
    }

    public class RetailerSettings : RetailerSettingsBase
    {
        private readonly IStorageReaderWriter _StorageReaderWriter;

        public RetailerSettings() : base()
        {
            _StorageReaderWriter = new LocalStorageReaderWriter("intermarche");
        }

        public override async Task LoadAsync()
        {
            PersistentRetailerSettings persistentSettings = await _StorageReaderWriter.ReadAsync<PersistentRetailerSettings>();

            ClearUserStores();

            if (persistentSettings == null || persistentSettings.UserStoreIds == null)
            {
                return;
            }

            foreach (var id in persistentSettings.UserStoreIds)
            {
                var store = await ((Retailer)Retailer).Locator.GetStoreFromIdAsync(id);
                if (store != null)
                {
                    AddUserStore(store);
                }
            }
        }

        public override async Task SaveAsync()
        {
            PersistentRetailerSettings persistentSettings = new PersistentRetailerSettings
            {
                UserStoreIds = _UserStores.Select(store => store.Id).ToArray()
            };

            await _StorageReaderWriter.WriteAsync(persistentSettings);
        }
    }
}
