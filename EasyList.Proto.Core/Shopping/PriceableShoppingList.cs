using EasyList.Proto.Core.Misc.Containers;
using EasyList.Proto.Core.Retailers;

namespace EasyList.Proto.Core.Shopping
{
    public class PriceableShoppingList : ObservableContainer<PriceableShoppingListItem>
    {
        public IStore Store { get; }

        private float _Price;
        public float Price
        {
            get
            {
                return _Price;
            }

            set
            {
                SetProperty(ref _Price, value);
            }
        }

        public PriceableShoppingList(ShoppingList shoppingList, IStore store)
        {
            Store = store;

            foreach (var item in shoppingList)
            {
                if(item.IsIncluded)
                    Add(new PriceableShoppingListItem(item));
            }
        }
    }
}
