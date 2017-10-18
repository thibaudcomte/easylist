using EasyList.Proto.Core.Retailers;
using EasyList.Proto.Core.Shopping;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System.Windows.Input;

namespace EasyList.Proto.ViewModels
{
    class ShoppingListPageViewModel : ViewModelBase
    {
        public ShoppingList ShoppingList { get; }

        public ICommand PriceShoppingListCommand { get; }

        public ShoppingListPageViewModel(ShoppingFacade shoppingFacade, RetailersFacade retailersManager, INavigationService navigationService)
        {
            ShoppingList = shoppingFacade.ShoppingList;

            PriceShoppingListCommand = new DelegateCommand(() =>
            {
                navigationService.Navigate(PageTokens.Pricing.ToString(), null);
            });
        }
    }
}
