using EasyList.Proto.Core.Misc.Mvvm;
using EasyList.Proto.Core.Recipes;

namespace EasyList.Proto.Core.Shopping
{
    public class ShoppingListItem : BindableBase
    {
        public Ingredient Ingredient { get; }

        private float _OrderAmount;
        public float OrderAmount
        {
            get { return _OrderAmount; }
            set { SetProperty(ref _OrderAmount, value); }
        }

        private bool _IsIncluded;
        public bool IsIncluded
        {
            get { return _IsIncluded; }
            set
            {
                if (SetProperty(ref _IsIncluded, value))
                {
                    OrderAmount = IsIncluded ? _OriginalAmount : 0;
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
    }
}
