using EasyList.Proto.Core.Shopping;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EasyList.Proto.Core.Retailers
{
    public interface IRetailerShoppingSession
    {
        IStore Store { get; }
        Cookie[] RetailerShoppingSessionCookies { get; }
        Task<PricedShoppingList> PriceShoppingListAsync(ShoppingList list);
        Task PriceShoppingListAsync(PricedShoppingList list);
    }
}
