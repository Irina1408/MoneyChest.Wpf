using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MoneyChest.ViewModel.Commands
{
    public class ParametrizedCommand<T> : ICommand
    {
        #region Private fields

        private Action<T> execute;
        private Func<T, bool> canExecute;

        #endregion

        #region Initialization

        public ParametrizedCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        #endregion

        #region ICommand implementation

        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke((T)parameter) ?? true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            execute?.Invoke((T)parameter);
        }

        #endregion
    }
}
