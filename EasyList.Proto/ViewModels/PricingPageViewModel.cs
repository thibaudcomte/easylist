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
    class RetailerShoppingSessionInfo : BindableBase
    {
        public IRetailerShoppingSession RetailerShoppingSession { get; }
        public PriceableShoppingList PricedShoppingList { get; }

        private bool _IsPricingComplete;
        public bool IsPricingComplete
        {
            get { return _IsPricingComplete; }
            set { SetProperty(ref _IsPricingComplete, value); }
        }

        public RetailerShoppingSessionInfo(IRetailerShoppingSession retailerShoppingSession, ShoppingList shoppingList)
        {
            RetailerShoppingSession = retailerShoppingSession;
            PricedShoppingList = new PriceableShoppingList(shoppingList, retailerShoppingSession.Store);
        }

        public async Task PriceShoppingListAsync()
        {
            IsPricingComplete = false;
            await RetailerShoppingSession.PriceShoppingListAsync(PricedShoppingList);
            IsPricingComplete = true;
        }
    }

    class PricingPageViewModel : ViewModelBase
    {
        public ObservableCollection<RetailerShoppingSessionInfo> RetailerShoppingSessionInfos { get; }

        public DelegateCommand<RetailerShoppingSessionInfo> GoToOnlineShoppingCartCommand { get; }

        public PricingPageViewModel(ShoppingFacade shoppingFacade, RetailersFacade retailersFacade, INavigationService navigationService)
        {
            RetailerShoppingSessionInfos = new ObservableCollection<RetailerShoppingSessionInfo>();

            foreach (var store in retailersFacade.UserStoresContainer)
            {
                RetailerShoppingSessionInfos.Add(new RetailerShoppingSessionInfo(
                    store.Retailer.Shopper.CreateRetailerShoppingSession(store), shoppingFacade.ShoppingList));
            }

            GoToOnlineShoppingCartCommand = new DelegateCommand<RetailerShoppingSessionInfo>(sessionInfo =>
            {
                navigationService.Navigate(PageTokens.OnlineCart.ToString(), sessionInfo);
            });
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
