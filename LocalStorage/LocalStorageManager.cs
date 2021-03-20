using Blazored.LocalStorage.StorageOptions;
using Energetic.Clients.LocalStorage;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Energetic.Clients.BlazorWasm.LocalStorage
{
    public class LocalStorageManager : Blazored.LocalStorage.LocalStorageService, ILocalStorageManager
    {
        public LocalStorageManager(IJSRuntime jsRuntime, IOptions<LocalStorageOptions> options) : base(jsRuntime, options)
        {
        }
    }
}