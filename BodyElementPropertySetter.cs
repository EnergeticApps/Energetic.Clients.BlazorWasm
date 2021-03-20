using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Energetic.Clients.BlazorWasm
{
    public class BodyElementPropertySetter : ComponentBase
    {
        private readonly IJSRuntime _jsRuntime;


        // ToDo: ensure that the relevant .js files get included in the <head> section of the page

        public BodyElementPropertySetter(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        }


        [Parameter]
        public string? CssClass { get; set; } = null;

        [Parameter]
        public string? Language { get; set; } = null;

        [Parameter]
        public string? TextDirection { get; set; } = null;

        protected async override Task OnParametersSetAsync()
        {
            if (CssClass is { })
                await _jsRuntime.InvokeVoidAsync("addCssClassToBody", CssClass);

            if (Language is { })
                await _jsRuntime.InvokeVoidAsync("setLanguageOfBody", Language);

            if (TextDirection is { })
                await _jsRuntime.InvokeVoidAsync("setTextDirectionOfBody", TextDirection);
        }
    }
}
