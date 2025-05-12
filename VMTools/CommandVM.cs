using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace wpfHUSH.VMTools
{
    public class CommandVM : ICommand
    {
        Action action;
        Func<bool> canExecute;

        public CommandVM(Action action, Func<bool> canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public bool CanExecute(object? parameter)
        {
            return canExecute();
        }
        public void Execute(object? parameter)
        {
            action();
        }
    }

    public class CommandVM<T> : ICommand
    {
        Action<T> action;
        Func<bool> canExecute;

        public CommandVM(Action<T> action, Func<bool> canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public bool CanExecute(object? parameter)
        {
            return canExecute();
        }
        public void Execute(object? parameter)
        {
            action((T)parameter);
        }
    }
}
