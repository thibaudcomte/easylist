using System.ComponentModel;

namespace EasyList.Proto.Core.Shopping
{
    public class PricedShoppingListItem : INotifyPropertyChanged
    {
        public ShoppingListItem ShoppingListItem { get; }

        private float _Price;

        public float Price
        {
            get { return _Price; }
            set
            {
                if (value != _Price)
                {
                    _Price = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
                }
            }
        }

        public PricedShoppingListItem(ShoppingListItem shoppingListItem)
        {
            ShoppingListItem = shoppingListItem;
            Price = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
