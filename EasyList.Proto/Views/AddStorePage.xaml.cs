using EasyList.Proto.Core.Retailers;
using EasyList.Proto.ViewModels;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EasyList.Proto.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddStorePage : Page
    {
        public AddStorePage()
        {
            InitializeComponent();
        }

        internal AddStorePageViewModel ConcreteDataContext => DataContext as AddStorePageViewModel;

        private async void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                await ConcreteDataContext.UpdateSearchAsync(sender.Text);
            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var store = (IStore)args.SelectedItem;
            sender.Text = store.City;
        }

        private async void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                // User selected an item from the suggestion list, take an action on it here.
                var store = (IStore)args.ChosenSuggestion;
                ConcreteDataContext.SubmitSearch(store);
            }
            else
            {
                // Use args.QueryText to determine what to do.
                await ConcreteDataContext.SubmitSearchAsync(args.QueryText);
            }
        }
    }
}
