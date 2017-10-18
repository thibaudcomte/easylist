using EasyList.Proto.Core.Retailers;
using EasyList.Proto.Core.Shopping;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasyList.Proto.ViewModels
{
    class RetailerShoppingSessionInfo
    {
        public IRetailerShoppingSession RetailerShoppingSession { get; }
        public PricedShoppingList PricedShoppingList { get; }

        public RetailerShoppingSessionInfo(IRetailerShoppingSession retailerShoppingSession, ShoppingList shoppingList)
        {
            RetailerShoppingSession = retailerShoppingSession;
            PricedShoppingList = new PricedShoppingList(shoppingList, retailerShoppingSession.Store);
        }

        public Task PriceShoppingListAsync()
        {
            return RetailerShoppingSession.PriceShoppingListAsync(PricedShoppingList);
        }
    }

    class PricingPageViewModel : ViewModelBase
    {
        public ObservableCollection<RetailerShoppingSessionInfo> RetailerShoppingSessionInfos { get; }

        private RetailerShoppingSessionInfo _SelectedRetailerShoppingSessionInfo;
        public RetailerShoppingSessionInfo SelectedRetailerShoppingSessionInfo
        {
            get { return _SelectedRetailerShoppingSessionInfo; }
            set { SetProperty(ref _SelectedRetailerShoppingSessionInfo, value); }
        }

        public ICommand GoToOnlineShoppingCartCommand { get; }

        public PricingPageViewModel(ShoppingFacade shoppingFacade, RetailersFacade retailersFacade, INavigationService navigationService)
        {
            RetailerShoppingSessionInfos = new ObservableCollection<RetailerShoppingSessionInfo>();

            foreach (var store in retailersFacade.UserStoresContainer)
            {
                RetailerShoppingSessionInfos.Add(new RetailerShoppingSessionInfo(
                    store.Retailer.Shopper.CreateRetailerShoppingSession(store), shoppingFacade.ShoppingList));
            }

            GoToOnlineShoppingCartCommand = new DelegateCommand(() =>
            {
                navigationService.Navigate("OnlineCart", SelectedRetailerShoppingSessionInfo);
            }, () => { return SelectedRetailerShoppingSessionInfo != null; }).ObservesProperty(() => SelectedRetailerShoppingSessionInfo);
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);

            List<Task> tasks = new List<Task>();
            foreach (var info in RetailerShoppingSessionInfos)
            {
                tasks.Add(info.PriceShoppingListAsync());
            }

            await Task.WhenAll(tasks);
        }
    }
}
