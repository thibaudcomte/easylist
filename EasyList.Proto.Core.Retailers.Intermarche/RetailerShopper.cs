
using EasyList.Proto.Core.Retailers;
using EasyList.Proto.Core.Retailers.Intermarche;
using EasyList.Proto.Core.Shopping;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Web.Http;

namespace EasyList.Proto.Core.Intermarche
{
    class CartInfo
    {
        public float ItemPrice { get; internal set; }
        public float TotalPrice { get; internal set; }
    }

    public class RetailerShopper : IRetailerShopper, IDisposable
    {
        private HttpClient _HttpClient = new HttpClient();
        private readonly CultureInfo _CultureFR = new CultureInfo("fr-FR");

        public async Task PriceShoppingListAsync(PricedShoppingList list)
        {
            Store typedStore = list.Store as Store;

            await InitializeTransactionAsync(typedStore);

            foreach (var item in list.PricedShoppingListItems)
            {
                if (item.ShoppingListItem.IsIncluded)
                {
                    CartInfo cartInfo = await AddItemToCart(typedStore, item.ShoppingListItem);
                    item.Price = cartInfo.ItemPrice;
                    list.Price = cartInfo.TotalPrice;
                }
            }
        }

        private async Task InitializeTransactionAsync(Store store)
        {
            // get initial cookies.
            // the magic is done by the UWP as it stores all cookies in the app context
            // that WebView also uses.

            var response = await _HttpClient.GetAsync(new Uri($"https://drive.intermarche.com/{store.Urlh}"));

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to connect to store {store} (status code is {response.StatusCode}).");
            }
        }

        private async Task<int> GetProductIdForItemAsync(Store store, ShoppingListItem item)
        {
            string content = await _HttpClient.GetStringAsync(new Uri($"https://drive.intermarche.com/{store.Urlh}/produit/recherche/{item.Ingredient.Name}"));
            Match match = Regex.Match(content, @"list_product_id: ""(\d+)""");
            string id = match.Groups[1].Value;
            return int.Parse(id);
        }

        private async Task<CartInfo> AddItemToCart(Store store, ShoppingListItem item)
        {
            int id = await GetProductIdForItemAsync(store, item);

            using (var request = new HttpRequestMessage(HttpMethod.Post, new Uri("https://drive.intermarche.com/Plus")))
            {
                request.Content = new HttpStringContent($"{{'idProduit' : '{id}'}}",
                    Windows.Storage.Streams.UnicodeEncoding.Utf8,
                    "application/json");

                request.Headers.Accept.Add(Windows.Web.Http.Headers.HttpMediaTypeWithQualityHeaderValue.Parse("application/json"));

                using (var response = await _HttpClient.SendRequestAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Failed to add item to basket (store is {store}, status code is {response.StatusCode}).");
                    }

                    CartInfo cartInfo = new CartInfo();

                    JsonObject jsonResponse = JsonValue.Parse(response.Content.ToString()).GetObject();
                    cartInfo.ItemPrice = float.Parse(jsonResponse["PrixArticle"].GetString(), NumberStyles.Currency, _CultureFR);
                    cartInfo.TotalPrice = float.Parse(jsonResponse["MontantFinal"].GetString(), NumberStyles.Currency, _CultureFR);

                    return cartInfo;
                }
            }
        }

        public void Dispose()
        {
            _HttpClient.Dispose();
        }

        
    }
}
