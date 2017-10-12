using EasyList.Proto.Core.Containers;
using EasyList.Proto.Core.Retailers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyList.Proto.Core.Shopping
{
    public class PricedShoppingList : INotifyPropertyChanged
    {
        public IStore Store { get; }
        public ObservableContainer<PricedShoppingListItem> PricedShoppingListItems { get; }
        private float _Price;
        public float Price
        {
            get { return _Price; }
            set
            {
                if (value != Price)
                {
                    _Price = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
                }
            }
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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
