using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MoneyChest.View.Commands
{
    public class DataGridSelectedItemsCommand<T> : ICommand, IDisposable
        where T : class
    {
        #region Private fields

        private DataGrid dataGrid;
        private Action<IEnumerable<T>> execute;
        private Func<IEnumerable<T>, bool> canExecute;

        #endregion

        #region Initialization

        public DataGridSelectedItemsCommand(DataGrid dataGrid, Action<IEnumerable<T>> execute, Func<IEnumerable<T>, bool> canExecute = null)
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

        #region Public methods

        public void ValidateCanExecute()
        {
            if (canExecute != null && CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

        #endregion

        #region ICommand implementation

        public bool CanExecute(object parameter)
        {
            if (dataGrid.SelectedItems == null || dataGrid.SelectedItems.Count == 0)
                return false;
            return canExecute == null || canExecute(dataGrid.SelectedItems.OfType<T>());
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (dataGrid.SelectedItems != null && dataGrid.SelectedItems.Count > 0)
                execute?.Invoke(dataGrid.SelectedItems.OfType<T>());
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
