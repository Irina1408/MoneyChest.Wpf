using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MoneyChest.ViewModel.Commands
{
    public class TreeViewSelectedItemCommand<T> : IMCCommand, IDisposable
         where T : class
    {
        #region Private fields

        private TreeView treeView;
        private Action<T> execute;
        private Func<T, bool> canExecute;

        #endregion

        #region Initialization

        public TreeViewSelectedItemCommand(TreeView treeView, Action<T> execute, Func<T, bool> canExecute = null)
        {
            this.treeView = treeView;
            this.execute = execute;
            this.canExecute = canExecute;
            this.treeView.SelectedItemChanged += treeView_SelectedItemChanged;
        }

        private void treeView_SelectedItemChanged(object sender, EventArgs e)
        {
            ValidateCanExecute();
        }

        #endregion

        #region Public methods

        public void ValidateCanExecute()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region ICommand implementation

        public bool CanExecute(object parameter)
        {
            if (treeView.SelectedItem as T == null)
                return false;

            return canExecute?.Invoke(treeView.SelectedItem as T) ?? true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (treeView.SelectedItem as T != null)
                execute?.Invoke(treeView.SelectedItem as T);
        }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            this.treeView.SelectedItemChanged -= treeView_SelectedItemChanged;
        }

        #endregion
    }
}
