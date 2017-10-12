using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyList.Proto.Core.Retailers
{
    public interface IRetailerLocator
    {
        Task<IEnumerable<IStore>> FindStoresAsync(string query);
    }
}
