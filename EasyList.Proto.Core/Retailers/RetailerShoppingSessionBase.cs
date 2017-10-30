using System;
using System.Net;
using System.Threading.Tasks;
using EasyList.Proto.Core.Shopping;
using System.Net.Http;

namespace EasyList.Proto.Core.Retailers
{
    public abstract class RetailerShoppingSessionBase : IRetailerShoppingSession, IDisposable
    {
        public IStore Store { get; }
        public Cookie[] Cookies => GetSessionCookies();

        protected HttpClientHandler _HttpClientHandler;
        protected HttpClient _HttpClient;

        public RetailerShoppingSessionBase(IStore store)
        {
            Store = store;
        }

        protected virtual Task InitializeAsync()
        {
            DisposeHttpResources();

            _HttpClientHandler = new HttpClientHandler();
            _HttpClient = new HttpClient(_HttpClientHandler);

            return Task.CompletedTask;
        }

        public abstract Task PriceShoppingListAsync(PriceableShoppingList list);

        protected abstract Cookie[] GetSessionCookies();

        public void Dispose()
        {
            DisposeHttpResources();
        }

        private void DisposeHttpResources()
        {
            _HttpClient?.Dispose();
            _HttpClientHandler?.Dispose();
        }
    }
}
