using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyList.Proto.Core.Retailers
{
    public interface IRetailerSettings
    {
        IEnumerable<IStore> UserStores { get; }
        Task SaveAsync();
        Task LoadAsync();
        void AddUserStore(IStore store);
        void RemoveUserStore(IStore store);
        void ClearUserStores();
    }
}
