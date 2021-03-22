using Blazored.LocalStorage.StorageOptions;
using Energetic.Clients.LocalStorage;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Energetic.Clients.BlazorWasm.LocalStorage
{
    /// <summary>
    /// A local storage manager for Blazor WebAssembly clients.
    /// </summary>
    /// <remarks>
    /// This is just a wrapper for the Blazored LocalStorageService, implementing our own interface ILocalStorageManager, allowing it to
    /// be used interchangeably with equivalent implementations for Xamarin.
    /// </remarks>
    public class LocalStorageManager : Blazored.LocalStorage.LocalStorageService, ILocalStorageManager
    {
        /// <summary>
        /// Constructor for this class
        /// </summary>
        /// <param name="jsRuntime">A reference to a <see cref="IJSRuntime"/> that can be used to run JavaScript on the user's browser.</param>
        /// <param name="options">Options for configuring behaviour.</param>
        public LocalStorageManager(IJSRuntime jsRuntime, IOptions<LocalStorageOptions> options) : base(jsRuntime, options)
        {
        }
    }
}