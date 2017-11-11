using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MoneyChest.View.Commands
{
    public class DataGridSelectedItemCommand<T> : IMCCommand, IDisposable
         where T : class
    {
        #region Private fields

        private DataGrid dataGrid;
        private Action<T> execute;
        private Func<T, bool> canExecute;

        #endregion

        #region Initialization

        public DataGridSelectedItemCommand(DataGrid dataGrid, Action<T> execute, Func<T, bool> canExecute = null)
        {
            this.dataGrid = dataGrid;
            this.execute = execute;
            this.canExecute = canExecute;
            this.dataGrid.SelectionChanged += dataGrid_SelectionChanged;
        }

        private void dataGrid_SelectionChanged(object sender, EventArgs e)
        {
            ValidateCanExecute();
        }

        #endregion

        #region IMCCommand implementation

        public void ValidateCanExecute()
        {
            if (canExecute != null && CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

        #endregion

        #region ICommand implementation

        public bool CanExecute(object parameter)
        {
            if (dataGrid.SelectedItem == null || dataGrid.SelectedItems == null || dataGrid.SelectedItems.Count != 1)
                return false;
            return canExecute == null || canExecute(dataGrid.SelectedItem as T);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (dataGrid.SelectedItem != null)
                execute?.Invoke(dataGrid.SelectedItem as T);
        }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            this.dataGrid.SelectionChanged -= dataGrid_SelectionChanged;
        }

        #endregion
    }
}
