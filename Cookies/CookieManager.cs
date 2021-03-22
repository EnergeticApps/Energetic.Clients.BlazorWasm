using Energetic.Clients.Cookies;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Energetic.Clients.BlazorWasm.Cookies
{
    /// <summary>
    /// A cookie manager for Blazor WebAssembly apps. 
    /// </summary>
    /// <remarks>The current implementation requires a javascript to be placed in the wwwroot directory.
    /// Future implementations should register that automatically if possible.</remarks>
    public class CookieManager : ICookieManager
    {
        private readonly IJSRuntime _javascript;

        /// <summary>
        /// Constructor for this class.
        /// </summary>
        /// <param name="jsRuntime">A reference to a <see cref="IJSRuntime"/> that can be used to run JavaScript on the user's browser.</param>
        public CookieManager(IJSRuntime jsRuntime)
        {
            _javascript = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        }

        /// <summary>
        /// Clears all our app's cookies for this user.
        /// </summary>
        public async ValueTask ClearAsync()
        {
            await _javascript.InvokeVoidAsync("");
        }

        /// <summary>
        /// Checks to see if this user has any cookie identifiable by the name passed in the parameter <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The name of the cookie</param>
        /// <returns>True if a cookie exists by that name, otherwise false.</returns>
        public async ValueTask<bool> ContainsKeyAsync(string key)
        {
            return string.IsNullOrEmpty(await GetItemAsStringAsync(key));
        }

        /// <summary>
        /// Gets the value of the named cookie as a string.
        /// </summary>
        /// <param name="key">The name of the cookie.</param>
        /// <returns>The value of the cookie.</returns>
        public async ValueTask<string?> GetItemAsStringAsync(string key)
        {
            var cookie = await _javascript.InvokeAsync<string>("getCookie", new object[] { key });
            if (cookie is null)
                return default;

            return cookie.ToString();
        }

        /// <summary>
        /// Gets the value of the named cookie cast to a type of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="key">The name of the cookie.</param>
        /// <typeparam name="T">The type to which the cookies should be cast before it is returned.</typeparam>
        /// <returns>The value of the cookie.</returns>
        public async ValueTask<T?> GetItemAsync<T>(string key)
        {
            var cookie = await _javascript.InvokeAsync<object>("getCookie", new object[] { key });

            Console.WriteLine($"The cookie {key} is {cookie}.");
            if (cookie is null || string.IsNullOrWhiteSpace(cookie.ToString()))
            {
                return default;
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

        /// <summary>
        /// Removes the cookie identified by the name passed in the argument <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The name of the cookie</param>
        public async ValueTask RemoveItemAsync(string key)
        {
            await SetItemAsync(key, string.Empty, -1);
        }

        /// <summary>
        /// Sets a cookie of type <typeparamref name="T"/> with the name of <paramref name="key"/> and a value of
        /// <paramref name="data"/>, which will expire if it is still there after <paramref name="expiryDays"/> days.
        /// </summary>
        /// <typeparam name="T">The type of data to be stored in the cookie.</typeparam>
        /// <param name="key">The name of the cookie.</param>
        /// <param name="data">The value to be stored in the cookie.</param>
        /// <param name="expiryDays">The number of days after which the cookie will expire.</param>
        public async ValueTask SetItemAsync<T>(string key, T data, int expiryDays)
        {
            DateTimeOffset expires = DateTimeOffset.UtcNow.AddDays(expiryDays);
            await SetItemAsync(key, data, expires);
        }

        /// <summary>
        /// Sets a cookie of type <typeparamref name="T"/> with the name of <paramref name="key"/> and a value of
        /// <paramref name="data"/>, which will expire if it is still there at <paramref name="expires"/>.
        /// </summary>
        /// <typeparam name="T">The type of data to be stored in the cookie.</typeparam>
        /// <param name="key">The name of the cookie.</param>
        /// <param name="data">The value to be stored in the cookie.</param>
        /// <param name="expires">The date and time at which the cookie should expire.</param>
        public async ValueTask SetItemAsync<T>(string key, T data, DateTimeOffset? expires = null)
        {
            if (data is null)
                throw new InvalidOperationException(
                    $"Can't set a cookie to null. If you want to delete cookies, call {nameof(RemoveItemAsync)} or {nameof(ClearAsync)} instead.");

            await _javascript.InvokeAsync<object>("setCookie", new object[] { key, data, expires! });
        }
    }
}