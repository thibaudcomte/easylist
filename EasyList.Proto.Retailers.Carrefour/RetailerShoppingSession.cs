using EasyList.Proto.Core.Retailers;
using EasyList.Proto.Core.Shopping;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EasyList.Proto.Retailers.Carrefour
{
    class CartInfo
    {
        public float ItemPrice { get; internal set; }
        public float TotalPrice { get; internal set; }
    }

    class ProductInfo
    {
        public string EAN { get; internal set; }
        public string ImageUrl { get; internal set; }
        public string Packaging { get; internal set; }
        public float Price { get; internal set; }
        public int Id { get; internal set; }
        public int Quantity { get; internal set; }
        public int QuantityMin { get; internal set; }
        public int QuantityMax { get; internal set; }
        public int QuantityStep { get; internal set; }
        public string Title { get; internal set; }

        public string GetFormBodyString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("ean=");
            builder.Append(EAN);
            builder.Append("&");

            builder.Append("img=");
            builder.Append(WebUtility.UrlEncode(ImageUrl));
            builder.Append("&");

            builder.Append("packaging=");
            builder.Append(WebUtility.UrlEncode(Packaging));
            builder.Append("&");

            builder.Append("parentId=&");

            builder.Append("price=");
            builder.Append(Price);
            builder.Append("&");

            builder.Append("productId=");
            builder.Append(Id);
            builder.Append("&");

            builder.Append("qte=");
            builder.Append(Quantity);
            builder.Append("&");

            builder.Append("qteMax=");
            builder.Append(QuantityMax);
            builder.Append("&");

            builder.Append("qteMin=");
            builder.Append(QuantityMin);
            builder.Append("&");

            builder.Append("step=");
            builder.Append(QuantityStep);
            builder.Append("&");

            builder.Append("title=");
            builder.Append(WebUtility.UrlEncode(Title));

            return builder.ToString();
        }
    }

    class RetailerShoppingSession : RetailerShoppingSessionBase
    {
        private readonly Store _Store;

        public RetailerShoppingSession(Store store) : base(store)
        {
            _Store = store;
        }

        public override async Task PriceShoppingListAsync(PriceableShoppingList list)
        {
            await InitializeAsync();

            foreach (var item in list)
            {
                CartInfo cartInfo = await AddItemToCart(item.ShoppingListItem);
                item.Price = cartInfo.ItemPrice;
                list.Price = cartInfo.TotalPrice;
            }
        }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _HttpClientHandler.CookieContainer.Add(new Uri("https://courses-en-ligne.carrefour.fr/"), 
                new Cookie("store", _Store.Id.ToString()));

            var response = await _HttpClient.GetAsync(new Uri($"https://courses-en-ligne.carrefour.fr/"));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to connect to store {Store} (status code is {response.StatusCode}).");
            }

            response = await _HttpClient.GetAsync(new Uri($"https://courses-en-ligne.carrefour.fr/service/runtime.js"));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to connect to store {Store} (status code is {response.StatusCode}).");
            }
        }

        private async Task<ProductInfo> GetProductInfoAsync(ShoppingListItem shoppingListItem)
        {
            string content = await _HttpClient.GetStringAsync(
                new Uri($"https://courses-en-ligne.carrefour.fr/search?q={WebUtility.UrlEncode(shoppingListItem.Ingredient.Name)}"));
            var parser = new AngleSharp.Parser.Html.HtmlParser();
            var document = parser.Parse(content);
            var cellSelector = "div.cd-ProductInfosActions.cd-ProductData";
            var cell = document.QuerySelector(cellSelector);

            return new ProductInfo
            {
                ImageUrl = cell.Attributes["data-img"].Value,
                Id = int.Parse(cell.Attributes["data-id"].Value),
                Quantity = 1,
                QuantityMax = int.Parse(cell.Attributes["data-qte-max"].Value),
                QuantityMin = int.Parse(cell.Attributes["data-qte-min"].Value),
                QuantityStep = int.Parse(cell.Attributes["data-step"].Value),
                Price = float.Parse(cell.Attributes["data-price"].Value),
                Packaging = cell.Attributes["data-packaging"].Value,
                Title = cell.Attributes["data-title"].Value,
                EAN = cell.Attributes["data-ean"].Value
            };
        }

        private async Task<CartInfo> AddItemToCart(ShoppingListItem shoppingListItem)
        {
            ProductInfo productInfo = await GetProductInfoAsync(shoppingListItem);
            string parameters = productInfo.GetFormBodyString();

            using (var request = new HttpRequestMessage(HttpMethod.Post, new Uri("https://courses-en-ligne.carrefour.fr/addtocart")))
            {
                request.Content = new StringContent(parameters, Encoding.UTF8, "application/x-www-form-urlencoded");

                request.Headers.Add("X-Requested-With", "XMLHttpRequest");

                using (var response = await _HttpClient.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Failed to add item to basket, status code is {response.StatusCode}).");
                    }

                    CartInfo cartInfo = new CartInfo();

                    JObject jsonResponse = JObject.Parse(await response.Content.ReadAsStringAsync());
                    cartInfo.ItemPrice = float.Parse((string)jsonResponse["totalLastItemAdded"]);
                    cartInfo.TotalPrice = (float)jsonResponse["total"];

                    return cartInfo;
                }
            }
        }

        protected override Cookie[] GetSessionCookies()
        {
            CookieCollection cookieCollection = _HttpClientHandler.CookieContainer.GetCookies(new Uri("https://courses-en-ligne.carrefour.fr"));
            List<Cookie> cookies = new List<Cookie>();

            foreach (Cookie cookie in cookieCollection)
            {
                cookies.Add(cookie);
            }

            return cookies.ToArray();
        }

        
    }
}
