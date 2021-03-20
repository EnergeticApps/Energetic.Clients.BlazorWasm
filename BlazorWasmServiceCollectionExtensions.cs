using Energetic.Clients.BlazorWasm.Cookies;
using Energetic.Clients.BlazorWasm.LocalStorage;
using Energetic.Clients.BlazorWasm.Navigation;
using Energetic.Clients.BlazorWasm.ViewModels;
using Energetic.Clients.Cookies;
using Energetic.Clients.LocalStorage;
using Energetic.Clients.Navigation;
using Energetic.Clients.ViewModels;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BlazorWasmServiceCollectionExtensions
    {
        public static IServiceCollection AddBlazorClientServices(this IServiceCollection services)
        {
            return services
                .AddScoped<INavigationManager, NavigationManager>()
                .AddScoped<ICookieManager, CookieManager>()
                .AddScoped<ICommandFactory, CommandFactory>()
                .AddScoped<ILocalStorageManager, LocalStorageManager>();
        }
    }
}