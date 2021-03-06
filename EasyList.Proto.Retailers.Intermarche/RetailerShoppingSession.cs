﻿using EasyList.Proto.Core.Retailers;
using EasyList.Proto.Core.Shopping;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyList.Proto.Retailers.Intermarche
{
    class CartInfo
    {
        public float ItemPrice { get; internal set; }
        public float TotalPrice { get; internal set; }
    }

    class RetailerShoppingSession : RetailerShoppingSessionBase
    {
        private readonly Store _Store;
        private readonly CultureInfo _CultureFR = new CultureInfo("fr-FR");

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

            var response = await _HttpClient.GetAsync(new Uri($"https://drive.intermarche.com/{_Store.Urlh}"));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to connect to store {_Store} (status code is {response.StatusCode}).");
            }
        }

        private async Task<int> GetProductIdForItemAsync(ShoppingListItem shoppingListItem)
        {
            string content = await _HttpClient.GetStringAsync(new Uri($"https://drive.intermarche.com/{_Store.Urlh}/produit/recherche/{shoppingListItem.Ingredient.Name}"));
            Match match = Regex.Match(content, @"list_product_id: ""(\d+)""");
            string id = match.Groups[1].Value;
            return int.Parse(id);
        }

        private async Task<CartInfo> AddItemToCart(ShoppingListItem shoppingListItem)
        {
            int id = await GetProductIdForItemAsync(shoppingListItem);

            using (var request = new HttpRequestMessage(HttpMethod.Post, new Uri("https://drive.intermarche.com/Plus")))
            {
                request.Content = new StringContent($"{{'idProduit' : '{id}'}}", Encoding.UTF8, "application/json");

                request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                using (var response = await _HttpClient.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Failed to add item to basket, status code is {response.StatusCode}).");
                    }

                    CartInfo cartInfo = new CartInfo();

                    JObject jsonResponse = JObject.Parse(await response.Content.ReadAsStringAsync());
                    cartInfo.ItemPrice = float.Parse(jsonResponse["PrixArticle"].ToString(), NumberStyles.Currency, _CultureFR);
                    cartInfo.TotalPrice = float.Parse(jsonResponse["MontantFinal"].ToString(), NumberStyles.Currency, _CultureFR);

                    return cartInfo;
                }
            }
        }

        protected override Cookie[] GetSessionCookies()
        {
            CookieCollection cookieCollection = _HttpClientHandler.CookieContainer.GetCookies(
                new Uri("https://drive.intermarche.com"));

            List<Cookie> cookies = new List<Cookie>();

            foreach (Cookie cookie in cookieCollection)
            {
                cookies.Add(cookie);
            }

            return cookies.ToArray();
        }
    }
}
