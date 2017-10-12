using EasyList.Proto.Core.Shopping;
using System.Threading.Tasks;

namespace EasyList.Proto.Core.Retailers
{
    public interface IRetailerShopper
    {
        Task PriceShoppingListAsync(PricedShoppingList list);
    }
}
