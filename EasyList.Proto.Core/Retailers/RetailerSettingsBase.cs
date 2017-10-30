using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyList.Proto.Core.Retailers
{
    /// <summary>
    /// Serves as a base implementation to platform-specific ones.
    /// </summary>
    public abstract class RetailerSettingsBase<TRetailer, TStore> : IRetailerSettings
        where TRetailer : IRetailer
        where TStore : IStore
    {
        public TRetailer Retailer { get; set; }

        protected readonly List<TStore> _UserStores;
        IEnumerable<IStore> IRetailerSettings.UserStores => _UserStores as IEnumerable<IStore>;

        public RetailerSettingsBase()
        {
            _UserStores = new List<TStore>();
        }

        public abstract Task SaveAsync();
        public abstract Task LoadAsync();

        public void AddUserStore(IStore store)
        {
            if (store is TStore)
            {
                _UserStores.Add((TStore)store);
            }
        }

        public void RemoveUserStore(IStore store)
        {
            if (store is TStore)
            {
                _UserStores.Remove((TStore)store);
            }
        }

        public void ClearUserStores()
        {
            _UserStores.Clear();
        }
    }
}
