using EasyList.Proto.Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EasyList.Proto.Core.Retailers.Containers
{
    public class UserStoresContainerRetriever
    {
        private readonly UserStoresContainer _UserStoresContainer;
        private readonly IRetailersProvider _RetailersProvider;

        public UserStoresContainerRetriever(UserStoresContainer userStoresContainer, IRetailersProvider retailersProvider)
        {
            _UserStoresContainer = userStoresContainer;
            _RetailersProvider = retailersProvider;
        }

        public async Task LoadAsync()
        {
            await Task.WhenAll(_RetailersProvider.Retailers.Select(async retailer => await retailer.Settings.LoadAsync()));
            IEnumerable<IStore> userStores = _RetailersProvider.Retailers.SelectMany(retailer => retailer.Settings.UserStores);

            _UserStoresContainer.Clear();

            foreach (var store in userStores)
            {
                _UserStoresContainer.Add(store);
            }
        }

        public async Task SaveAsync()
        {
            foreach (var retailer in _RetailersProvider.Retailers)
            {
                retailer.Settings.ClearUserStores();
            }

            foreach (var store in _UserStoresContainer)
            {
                store.Retailer.Settings.AddUserStore(store);
            }

            foreach (var retailer in _RetailersProvider.Retailers)
            {
                await retailer.Settings.SaveAsync();
            }
        }
    }
}
