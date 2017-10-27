using EasyList.Proto.Core.Retailers;

namespace EasyList.Proto.Retailers.Carrefour
{
    public class RetailerShopper : IRetailerShopper
    {
        public IRetailerShoppingSession CreateRetailerShoppingSession(IStore store)
        {
            return new RetailerShoppingSession(store as Store);
        }
    }
}
