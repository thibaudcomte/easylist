using EasyList.Proto.Core.Retailers;
using EasyList.Proto.Core.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyList.Proto.Retailers.Intermarche
{
    /// <summary>
    /// Serves as a base implementation to platform-specific ones.
    /// </summary>
    public abstract class RetailerSettingsBase : IRetailerSettings
    {
        public Retailer Retailer { get; internal set; }
        protected readonly List<Store> _UserStores;
        IEnumerable<IStore> IRetailerSettings.UserStores => _UserStores as IEnumerable<IStore>;

        public RetailerSettingsBase()
        {
            _UserStores = new List<Store>();
        }

        public abstract Task SaveAsync();
        public abstract Task LoadAsync();

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
