using Energetic.Clients.Cookies;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Energetic.Clients.BlazorWasm.Cookies
{
    public class CookieManager : ICookieManager
    {
        private readonly IJSRuntime _javascript;

        public CookieManager(IJSRuntime jsRuntime)
        {
            _javascript = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        }

        public async ValueTask ClearAsync()
        {
            await _javascript.InvokeVoidAsync("");
        }

        public async ValueTask<bool> ContainsKeyAsync(string key)
        {
            return string.IsNullOrEmpty(await GetItemAsStringAsync(key));
        }

        public async ValueTask<string> GetItemAsStringAsync(string key)
        {
            var cookie = await _javascript.InvokeAsync<string>("getCookie", new object[] { key });
            if (cookie is null)
                return string.Empty;

            return cookie.ToString();
        }

        public async ValueTask<T> GetItemAsync<T>(string key)
        {
            var cookie = await _javascript.InvokeAsync<object>("getCookie", new object[] { key });

            Console.WriteLine($"The cookie {key} is {cookie}.");
            if (cookie is null || string.IsNullOrWhiteSpace(cookie.ToString()))
            {
                Console.WriteLine($"The cookie {key} was null.");
                Console.WriteLine($"Returning a value of {default(T)} for {key}.");
                return default(T);
            }

            try
            {
                var result = (T)cookie;
                Console.WriteLine($"The cookie {key} cast to a {typeof(T)} is {result}.");
                return result;
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidOperationException($"The {key} cookie couldn't be cast to type {typeof(T)}", ex);
            }
        }

        public async ValueTask RemoveItemAsync(string key)
        {
            await SetItemAsync(key, string.Empty, -1);
        }

        public async ValueTask SetItemAsync<T>(string key, T data, int expiryDays)
        {
            DateTimeOffset expires = DateTimeOffset.UtcNow.AddDays(expiryDays);
            await SetItemAsync(key, data, expires);
        }

        public async ValueTask SetItemAsync<T>(string key, T data, DateTimeOffset? expires = null)
        {
            if (data is null)
                throw new InvalidOperationException(
                    $"Can't set a cookie to null. If you want to delete cookies, call {nameof(RemoveItemAsync)} or {nameof(ClearAsync)} instead.");

            await _javascript.InvokeAsync<object>("setCookie", new object[] { key, data, expires! });
        }
    }
}