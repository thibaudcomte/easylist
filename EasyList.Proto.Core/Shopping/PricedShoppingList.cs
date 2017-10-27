using EasyList.Proto.Core.Misc.Containers;
using EasyList.Proto.Core.Misc.Mvvm;
using EasyList.Proto.Core.Retailers;

namespace EasyList.Proto.Core.Shopping
{
    public class PricedShoppingList : BindableBase
    {
        public IStore Store { get; }
        public ObservableContainer<PricedShoppingListItem> PricedShoppingListItems { get; }
        private float _Price;
        public float Price
        {
            get { return _Price; }
            set { SetProperty(ref _Price, value); }
        }

        public PricedShoppingList(ShoppingList shoppingList, IStore store)
        {
            Store = store;

            PricedShoppingListItems = new ObservableContainer<PricedShoppingListItem>();

            foreach (var item in shoppingList)
            {
                PricedShoppingListItems.Add(new PricedShoppingListItem(item));
            }
        }
    }
}
