using EasyList.Proto.ViewModels;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EasyList.Proto.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SelectedRecipesPage : Page, INotifyPropertyChanged
    {
        public SelectedRecipesPage()
        {
            InitializeComponent();

            DataContextChanged += delegate
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConcreteDataContext)));
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal SelectedRecipesPageViewModel ConcreteDataContext => DataContext as SelectedRecipesPageViewModel;
    }
}
