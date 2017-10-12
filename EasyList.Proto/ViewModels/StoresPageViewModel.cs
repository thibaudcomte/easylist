using EasyList.Proto.Core.Retailers;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EasyList.Proto.ViewModels
{
    class StoresPageViewModel : ViewModelBase
    {
        public ObservableCollection<IStore> UserStores { get; }

        public ICommand AddStoreCommand { get; }
        public ICommand DeleteStoreCommand { get; }
        public ICommand NavigateToStoreCommand { get; set; }

        private IStore _SelectedStore;
        public IStore SelectedStore
        {
            get { return _SelectedStore; }
            set { SetProperty(ref _SelectedStore, value); }
        }

        public StoresPageViewModel(INavigationService navigationService, RetailersFacade retailersFacade)
        {
            _RetailersFacade = retailersFacade;
            UserStores = _RetailersFacade.UserStoresContainer;

            AddStoreCommand = new DelegateCommand(() =>
            {
                navigationService.Navigate(PageTokens.AddStore.ToString(), null);
            });

            DeleteStoreCommand = new DelegateCommand(() =>
            {
                retailersFacade.UserStoresContainer.Remove(SelectedStore);
            }, () => { return SelectedStore != null; }).ObservesProperty(() => SelectedStore);

            NavigateToStoreCommand = new DelegateCommand(async () =>
            {
                Uri uri = new Uri($@"bingmaps:?rtp=~adr.{SelectedStore.Address},%20{SelectedStore.ZipCode},%20{SelectedStore.City}&amp;mode=d&amp;trfc=1");
                await Windows.System.Launcher.LaunchUriAsync(uri);
            }, () => { return SelectedStore != null; }).ObservesProperty(() => SelectedStore);
        }

        private RetailersFacade _RetailersFacade;
    }
}
