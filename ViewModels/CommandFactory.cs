using Energetic.Clients.ViewModels;
using GalaSoft.MvvmLight.Command;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Energetic.Clients.BlazorWasm.ViewModels
{
    public class CommandFactory : ICommandFactory
    {
        public ICommand CreateCommand(Action<object> execute)
        {
            return new RelayCommand<object>(execute);
        }

        public ICommand CreateCommand(Action execute)
        {
            return new RelayCommand(execute);
        }

        public ICommand CreateCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            return new RelayCommand<object>(execute, canExecute);
        }

        public ICommand CreateCommand(Action execute, Func<bool> canExecute)
        {
            return new RelayCommand(execute, canExecute);
        }

        public ICommand CreateCommand<T>(Action<T> execute)
        {
            return new RelayCommand<T>(execute);
        }

        public ICommand CreateCommand<T>(Action<T> execute, Func<T, bool> canExecute)
        {
            return new RelayCommand<T>(execute, canExecute);
        }

        public IAsyncCommand CreateCommand(Func<Task> execute, Func<bool>? canExecute = null, Action<Exception>? errorHandler = null)
        {
            return new AsyncCommand(execute, canExecute, errorHandler);
        }
    }
}