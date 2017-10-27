using EasyList.Proto.Core.Storage;
using EasyList.Proto.Core.Uwp.Storage.LocalStorage;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace EasyList.Proto.Retailers.Carrefour.Uwp
{
    [DataContract]
    public class PersistentStore
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string ZipCode { get; set; }
    }

    [DataContract]
    public class PersistentRetailerSettings
    {
        [DataMember]
        public PersistentStore[] Stores { get; set; }
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

            if (persistentSettings == null || persistentSettings.Stores == null)
            {
                return;
            }

            foreach (var persistentStore in persistentSettings.Stores)
            {
                Store store = new Store(Retailer, persistentStore.Id, persistentStore.Name, persistentStore.Address, persistentStore.City, persistentStore.ZipCode);
                AddUserStore(store);
            }
        }

        public override async Task SaveAsync()
        {
            PersistentRetailerSettings persistentSettings = new PersistentRetailerSettings
            {
                Stores = _UserStores.Select(store => new PersistentStore
                {
                    Id = store.Id,
                    Name = store.Name,
                    City = store.City,
                    Address = store.Address,
                    ZipCode = store.ZipCode
                }).ToArray()
            };

            await _StorageReaderWriter.WriteAsync(persistentSettings);
        }
    }
}
