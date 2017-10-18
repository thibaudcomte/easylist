using EasyList.Proto.Core.Shopping;
using System.Threading.Tasks;

namespace EasyList.Proto.Core.Retailers
{
    public interface IRetailerShopper
    {
        IRetailerShoppingSession CreateRetailerShoppingSession(IStore store);
    }
}
