using Energetic.Clients.Navigation;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Energetic.Clients.BlazorWasm.Navigation
{
    public class NavigationManager : NavigationManagerBase
    {
        private Microsoft.AspNetCore.Components.NavigationManager _aspNetCoreNavigationManager;
        private readonly IJSRuntime _javascript;

        public NavigationManager(Microsoft.AspNetCore.Components.NavigationManager aspNetCoreNavigationManager, IJSRuntime jsRuntime, IOptions<OidcProviderOptions> optionsAccessor) :
            base(optionsAccessor)
        {
            _aspNetCoreNavigationManager = aspNetCoreNavigationManager ?? throw new ArgumentNullException(nameof(aspNetCoreNavigationManager));
            _javascript = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        }

        public override string Location
        {
            get
            {
                return _aspNetCoreNavigationManager.Uri;
            }
        }

        public override async Task<bool> IsLoggedInAsync()
        {
            await Task.CompletedTask;
            //TODO: get this information properly.
            return false;
        }

        public override async Task NavigateToLogInAsync()
        {
            string pattern = "{0}/account/login?returnUrl={1}";
            string returnUrl = Uri.EscapeDataString(_aspNetCoreNavigationManager.Uri);
            await NavigateToPageAsync(MakeAuthUrl(pattern, returnUrl));
        }

        public override async Task NavigateToLogOutAsync()
        {
            await NavigateToPageAsync(MakeAuthUrl("{0}/account/logout"));
        }

        public override async Task NavigateToManageAccountAsync()
        {
            await NavigateToPageAsync(MakeAuthUrl("{0}/connect/userinfo"));
        }

        public override async Task NavigateToRegisterAsync()
        {
            await NavigateToLogInAsync();  // I think IdentityServer UI uses the same page for registering and logging in
        }

        public override async Task NavigateToPageAsync(string url)
        {
            _aspNetCoreNavigationManager.NavigateTo(url);
            await Task.CompletedTask;
        }

        public override async Task NavigateToPageAsync(string url, string parameterKey, string parameterValue)
        {
            _aspNetCoreNavigationManager.NavigateTo($"/{url}?{parameterKey}={parameterValue}");
            await Task.CompletedTask;
        }

        public override async Task NavigateExternalAsync(string url)
        {
            await _javascript.InvokeAsync<string>("openInNewTabOrWindow", new object[] { url });
        }
    }
}