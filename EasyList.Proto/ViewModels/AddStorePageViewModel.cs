using EasyList.Proto.Core.Retailers;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasyList.Proto.ViewModels
{
    class AddStorePageViewModel : BindableBase
    {
        public ObservableCollection<IStore> SearchResults { get; }

        private IStore _SelectedStore;
        public IStore SelectedStore
        {
            get { return _SelectedStore; }
            set { SetProperty(ref _SelectedStore, value); }
        }

        public ObservableCollection<IStore> SuggestionResults { get; }
        public ICommand AddUserStoreCommand { get; }

        public AddStorePageViewModel(RetailersFacade retailersFacade)
        {
            _RetailersFacade = retailersFacade;

            SearchResults = new ObservableCollection<IStore>();
            SuggestionResults = new ObservableCollection<IStore>();

            AddUserStoreCommand = new DelegateCommand(() =>
            {
                if (SelectedStore != null)
                {
                    retailersFacade.UserStoresContainer.Add(SelectedStore);
                }
            }, () => { return SelectedStore != null; }).ObservesProperty(() => SelectedStore);
        }

        internal async Task UpdateSearchAsync(string text)
        {
            // update the items to show in the suggest box

            if (text.Length < 3) return;

            SelectedStore = null;

            var results = await _RetailersFacade.FindStoresAsync(text);

            SuggestionResults.Clear();
            foreach (var result in results)
            {
                SuggestionResults.Add(result);
            }
        }

        internal async Task SubmitSearchAsync(string text)
        {
            // find the stores to show as a list

            SelectedStore = null;

            SearchResults.Clear();

            var results = await _RetailersFacade.FindStoresAsync(text);
            foreach (var result in results)
            {
                SearchResults.Add(result);
            }
        }

        internal void SubmitSearch(IStore store)
        {
            // the store has been singled-out by user

            SearchResults.Clear();
            SearchResults.Add(store);

            SelectedStore = store;
        }

        private RetailersFacade _RetailersFacade;
    }
}