using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MoneyChest.ViewModel.Commands
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

        public DataGridSelectedItemCommand(DataGrid dataGrid, Action<T> execute, Func<T, bool> canExecute = null, bool doubleClick = false)
        {
            this.dataGrid = dataGrid;
            this.execute = execute;
            this.canExecute = canExecute;
            this.dataGrid.SelectionChanged += dataGrid_SelectionChanged;

            if(doubleClick)
                this.dataGrid.MouseDoubleClick += dataGrid_MouseDoubleClick;
        }

        private void dataGrid_SelectionChanged(object sender, EventArgs e)
        {
            ValidateCanExecute();
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
                Execute(dataGrid.SelectedItem);
        }

        #endregion

        #region IMCCommand implementation

        public void ValidateCanExecute()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region ICommand implementation

        public bool CanExecute(object parameter)
        {
            if (dataGrid.SelectedItem == null || dataGrid.SelectedItems == null || dataGrid.SelectedItems.Count != 1)
                return false;
            return canExecute?.Invoke(dataGrid.SelectedItem as T) ?? true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (dataGrid.SelectedItem as T != null)
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
