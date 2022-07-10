using System;
using System.Windows.Input;

namespace HowToSpeak_DesktopClient.Infrastructure.Command.Base
{
    internal abstract class BaseCommand : ICommand
    {
        event EventHandler ICommand.CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;

            remove => CommandManager.RequerySuggested -= value;
        }

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);
    }
}
