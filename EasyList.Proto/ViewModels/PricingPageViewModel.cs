using EasyList.Proto.Core.Shopping;
using Prism.Mvvm;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Windows.Navigation;
using EasyList.Proto.Core.Retailers;
using System.Windows.Input;
using Prism.Commands;

namespace EasyList.Proto.ViewModels
{
    class PricingPageViewModel : ViewModelBase
    {
        public ObservableCollection<PricedShoppingList> PricedShoppingListList { get; }

        private PricedShoppingList _SelectedPriceShoppingList;
        public PricedShoppingList SelectedPriceShoppingList
        {
            get { return _SelectedPriceShoppingList; }
            set { SetProperty(ref _SelectedPriceShoppingList, value); }
        }

        public ICommand GoToOnlineShoppingCartCommand { get; }

        public PricingPageViewModel(ShoppingFacade shoppingFacade, RetailersFacade retailersFacade, INavigationService navigationService)
        {
            PricedShoppingListList = new ObservableCollection<PricedShoppingList>();

            foreach (var store in retailersFacade.UserStoresContainer)
            {
                PricedShoppingListList.Add(new PricedShoppingList(shoppingFacade.ShoppingList, store));
            }

            GoToOnlineShoppingCartCommand = new DelegateCommand(() =>
            {
                navigationService.Navigate("OnlineCart", SelectedPriceShoppingList.Store.CartUrl);
            }, () => { return SelectedPriceShoppingList != null; }).ObservesProperty(() => SelectedPriceShoppingList);
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);

            foreach (var list in PricedShoppingListList)
            {
                await list.Store.Retailer.Shopper.PriceShoppingListAsync(list);
            }
        }
    }
}
