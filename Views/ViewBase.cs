using Microsoft.AspNetCore.Components;
using Energetic.Clients.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Energetic.Clients.BlazorWasm.Views
{
    public class ViewBase<TViewModel> : ComponentBase, IDisposable
        where TViewModel : class, IViewModel
    {
        [Inject]
        protected TViewModel ViewModel { get; set; } = default!;

        public EventCallback BindToCommand(ICommand command)
        {
            if (command is null)
                throw new InvalidOperationException($"Can't bind a null reference of {typeof(ICommand)} to an {typeof(EventCallback)}.");

            var commandDelegate = new CommandDelegate(command.Execute);

            return new EventCallback(this, commandDelegate);
        }

        public EventCallback BindToCommand<T>(ICommand command, T parameter)
        {
            if (command is null)
                throw new InvalidOperationException($"Can't bind a null reference of {typeof(ICommand)} to an {typeof(EventCallback)}.");

            if (parameter is null)
                throw new InvalidOperationException($"This overload requires a parameter to be passed to the {typeof(ICommand)}.");

            return EventCallback.Factory.Create(this, () => command.Execute(parameter));
        }

        //private delegate void CommandDelegate();
        private delegate void CommandDelegate(object? parameter = null);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            //Free unmanaged resources

            if(disposing)
            {
                //Free managed resources

                if(ViewModel is { })
                {
                    ViewModel.PropertyChanged -= ViewModelPropertyChanged;
                    ViewModel.Dispose();
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            if (ViewModel is null)
                throw new InvalidOperationException("You can't tell the ViewModel to initialize itself if the ViewModel is still null!");

            // TODO: Confirm that this is the right place to tell the ViewModel to initialize itself.
            ViewModel.InitializeCommand.Execute(null); // Maybe we could pass a reference to the view here instead of null, but what's the point? The ViewModel will not know anything about the type of the view, which could be Blazor, Xamarin, WinForms or any [object]

            ViewModel.PropertyChanged += async (sender, e) => {
                await InvokeAsync(() =>
                {
                    ViewModelPropertyChanged(sender, e);
                });
            };

            await base.OnInitializedAsync();
        }

        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            StateHasChanged();
        }
    }
}
