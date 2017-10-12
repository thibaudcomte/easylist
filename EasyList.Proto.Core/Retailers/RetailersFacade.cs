using EasyList.Proto.Core.Retailers.Containers;
using EasyList.Proto.Core.Shopping;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyList.Proto.Core.Retailers
{
    public class RetailersFacade
    {
        private readonly IRetailersProvider _RetailersProvider;

        public UserStoresContainer UserStoresContainer { get; }

        public RetailersFacade(IRetailersProvider retailersProvider)
        {
            _RetailersProvider = retailersProvider;
            UserStoresContainer = new UserStoresContainer(retailersProvider);
        }

        public Task InitializeAsync()
        {
            return UserStoresContainer.LoadAsync();
        }

        public Task UnInitializeAsync()
        {
            return UserStoresContainer.SaveAsync();
        }

        public async Task<IEnumerable<IStore>> FindStoresAsync(string query)
        {
            var stores = new List<IStore>();

            foreach (var retailer in _RetailersProvider.Retailers)
            {
                stores.AddRange(await retailer.Locator.FindStoresAsync(query));
            }

            return stores;
        }
    }
}
