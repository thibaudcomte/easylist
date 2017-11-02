using EasyList.Proto.Core.Retailers;
using EasyList.Proto.Core.Shopping;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using EasyList.Proto.Core.Recipes;
using System.Collections;

namespace EasyList.Proto.ViewModels
{
    class ShoppingListCategoryGroup
    {
        public IngredientItemCategory Category { get; }
        public IEnumerable<ShoppingListItem> ShoppingListItems { get; }

        public ShoppingListCategoryGroup(IngredientItemCategory category, IEnumerable<ShoppingListItem> items)
        {
            Category = category;
            ShoppingListItems = items;
        }
    }

    class ShoppingListPageViewModel : ViewModelBase
    {
        public IEnumerable<ShoppingListCategoryGroup> ShoppingListCategoryGroups { get; }

        public ICommand PriceShoppingListCommand { get; }

        public ShoppingListPageViewModel(ShoppingFacade shoppingFacade, RetailersFacade retailersManager, INavigationService navigationService)
        {
            ShoppingListCategoryGroups = shoppingFacade.ShoppingList.GroupBy(item => item.Ingredient.Item.Category, 
                (cat, list) => new ShoppingListCategoryGroup(cat, list),
                new IngredientItemCategoryEqualityComparer());

            PriceShoppingListCommand = new DelegateCommand(() =>
            {
                navigationService.Navigate(PageTokens.Pricing.ToString(), null);
            });
        }
    }
}
