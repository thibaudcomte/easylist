using EasyList.Proto.Core.Shopping;
using System.Net;
using System.Threading.Tasks;

namespace EasyList.Proto.Core.Retailers
{
    public interface IRetailerShoppingSession
    {
        IStore Store { get; }
        Cookie[] Cookies { get; }
        Task PriceShoppingListAsync(PriceableShoppingList list);
    }
}
