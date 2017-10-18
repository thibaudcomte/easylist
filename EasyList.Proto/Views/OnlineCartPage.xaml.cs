using EasyList.Proto.ViewModels;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

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
            RetailerShoppingSessionInfo retailerShoppingSessionInfo = e.Parameter as RetailerShoppingSessionInfo;

            // creating the filter
            var myFilter = new HttpBaseProtocolFilter();
            myFilter.AllowAutoRedirect = true;

            // get a reference to the cookieManager (this applies to all requests)
            var cookieManager = myFilter.CookieManager;
            foreach (var cookie in retailerShoppingSessionInfo.RetailerShoppingSession.RetailerShoppingSessionCookies)
            {
                cookieManager.SetCookie(new HttpCookie(cookie.Name, cookie.Domain, cookie.Path) { Value = cookie.Value });
            }

            webView.Navigate(new Uri(retailerShoppingSessionInfo.RetailerShoppingSession.Store.CartUrl));

            base.OnNavigatedTo(e);
        }
    }
}
