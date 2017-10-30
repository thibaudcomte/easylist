using EasyList.Proto.Core.Misc.Mvvm;

namespace EasyList.Proto.Core.Shopping
{
    public class PriceableShoppingListItem : BindableBase
    {
        public ShoppingListItem ShoppingListItem { get; }

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

        public PriceableShoppingListItem(ShoppingListItem shoppingListItem)
        {
            ShoppingListItem = shoppingListItem;
        }
    }
}
