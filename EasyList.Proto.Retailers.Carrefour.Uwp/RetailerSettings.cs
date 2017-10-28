using EasyList.Proto.Core.Storage;
using EasyList.Proto.Core.Uwp.Storage.LocalStorage;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace EasyList.Proto.Retailers.Carrefour.Uwp
{
    [DataContract]
    public class PersistentRetailerSettings
    {
        [DataMember]
        public Store[] UserStores { get; set; }
    }

    public class RetailerSettings : RetailerSettingsBase
    {
        private readonly IStorageReaderWriter _StorageReaderWriter;

        public RetailerSettings() : base()
        {
            _StorageReaderWriter = new LocalStorageReaderWriter("carrefour");
        }

        public override async Task LoadAsync()
        {
            PersistentRetailerSettings persistentSettings = await _StorageReaderWriter.ReadAsync<PersistentRetailerSettings>();

            ClearUserStores();

            if (persistentSettings == null || persistentSettings.UserStores == null)
            {
                return;
            }

            foreach (var userStore in persistentSettings.UserStores)
            {
                userStore.Retailer = Retailer;
                AddUserStore(userStore);
            }
        }

        public override async Task SaveAsync()
        {
            PersistentRetailerSettings persistentSettings = new PersistentRetailerSettings
            {
                UserStores = _UserStores.ToArray()
            };

            await _StorageReaderWriter.WriteAsync(persistentSettings);
        }
    }
}
