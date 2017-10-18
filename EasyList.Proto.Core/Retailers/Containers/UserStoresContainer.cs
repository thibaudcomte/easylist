using EasyList.Proto.Core.Containers;
using System.Threading.Tasks;

namespace EasyList.Proto.Core.Retailers.Containers
{
    public class UserStoresContainer : ObservableContainer<IStore>
    {
        private readonly UserStoresContainerRetriever _UserStoresContainerRetriever;

        public UserStoresContainer(IRetailersProvider retailersProvider)
        {
            _UserStoresContainerRetriever = new UserStoresContainerRetriever(this, retailersProvider);
        }

        public Task LoadAsync()
        {
            return _UserStoresContainerRetriever.LoadAsync();
        }

        public Task SaveAsync()
        {
            return _UserStoresContainerRetriever.SaveAsync();
        }
    }
}
