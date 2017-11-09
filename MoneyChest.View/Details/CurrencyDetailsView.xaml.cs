using MoneyChest.Model.Model;
using MoneyChest.Services.Services;
using MoneyChest.Shared;
using MoneyChest.View.Commands;
using MoneyChest.View.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoneyChest.View.Details
{
    /// <summary>
    /// Interaction logic for CurrencyDetailsView.xaml
    /// </summary>
    public partial class CurrencyDetailsView : UserControl
    {
        #region Private fields

        private ICurrencyService _service;
        private EntityWrapper<CurrencyModel> _wrappedEntity;
        private bool _isNew;
        private DetailsViewCommandContainer _commands;
        private Action _closeAction;

        #endregion

        #region Initialization

        public CurrencyDetailsView(ICurrencyService service, CurrencyModel entity, bool isNew, Action closeAction)
        {
            InitializeComponent();

            // init
            _service = service;
            _isNew = isNew;
            _wrappedEntity = new EntityWrapper<CurrencyModel>(entity);
            _closeAction = closeAction;
            this.DataContext = _wrappedEntity.Entity;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            _commands = new DetailsViewCommandContainer()
            {
                SaveCommand = new Command(() =>
                {
                    if (_isNew)
                        _service.Add(_wrappedEntity.Entity);
                    else
                        _service.Update(_wrappedEntity.Entity);

                    // close control
                    _closeAction?.Invoke();
                }, 
                () => _wrappedEntity.IsChanged && !_wrappedEntity.HasErrors),

                CancelCommand = new Command(() =>
                {
                    if(_wrappedEntity.IsChanged)
                    {
                        if (MessageBox.Show("Are you sure you want to cancel changes?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            // revert changes
                            _wrappedEntity.RevertChanges();
                            // close control
                            _closeAction?.Invoke();
                        }
                    }
                    else
                        // close control
                        _closeAction?.Invoke();
                })
            };

            CommandsPanel.DataContext = _commands;
        }

        #endregion
    }
}
