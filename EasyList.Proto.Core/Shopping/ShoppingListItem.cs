using EasyList.Proto.Core.Recipes;
using System.ComponentModel;

namespace EasyList.Proto.Core.Shopping
{
    public class ShoppingListItem : INotifyPropertyChanged
    {
        public Ingredient Ingredient { get; }

        private float _OrderAmount;
        public float OrderAmount
        {
            get { return _OrderAmount; }
            set
            {
                if (_OrderAmount != value)
                {
                    _OrderAmount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OrderAmount)));
                }
            }
        }

        private bool _IsIncluded;
        public bool IsIncluded
        {
            get { return _IsIncluded; }
            set
            {
                if (value != _IsIncluded)
                {
                    _IsIncluded = value;
                    OrderAmount = IsIncluded ? _OriginalAmount : 0;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsIncluded)));
                }
            }
        }

        private readonly float _OriginalAmount;

        public ShoppingListItem(Ingredient ingredient, bool isIncluded)
        {
            Ingredient = ingredient;
            _IsIncluded = isIncluded;
            _OrderAmount = ingredient.Quantity;
            _OriginalAmount = ingredient.Quantity;
        }

        public ShoppingListItem(Ingredient ingredient) : this(ingredient, true)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
