using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EasyList.Proto.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OnlineCartPage : Page
    {
        public OnlineCartPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string url = e.Parameter.ToString();
            webView.Navigate(new Uri(url));
            base.OnNavigatedTo(e);
        }
    }
}
