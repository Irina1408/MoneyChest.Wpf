using MoneyChest.Services.Services.Base;
using MoneyChest.Shared.MultiLang;
using MoneyChest.ViewModel.Commands;
using MoneyChest.ViewModel.Wrappers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MoneyChest.View.Details
{
    public abstract class BaseEntityDetailsView<TEntity, TViewModel, TService> : UserControl
        where TEntity : class, INotifyPropertyChanged
        where TViewModel : class, TEntity, INotifyPropertyChanged
        where TService : IServiceBase<TEntity>
    {
        #region Protected fields

        protected TService _service;
        protected EntityWrapper<TViewModel> _wrappedEntity;
        protected bool _isNew;
        protected DetailsViewCommandContainer _commands;
        protected Action _closeAction;
        protected bool _closeView;

        #endregion

        #region Initialization
        
        public BaseEntityDetailsView()
        {
            // init
            _closeView = false;
            // attach event
            Loaded += DetailsView_Loaded;
        }

        public BaseEntityDetailsView(TService service, TViewModel entity, bool isNew, Action closeAction) : this()
        {
            // init
            _service = service;
            _isNew = isNew;
            _closeAction = closeAction;
            _wrappedEntity = new EntityWrapper<TViewModel>(entity);

            // init commands
            InitializeCommands();
        }

        protected virtual void InitializeCommands()
        {
            _commands = new DetailsViewCommandContainer()
            {
                SaveCommand = new Command(() =>
                {
                    // save changes
                    SaveChanges();
                    // close control
                    _closeAction?.Invoke();
                },
                () => _wrappedEntity.IsChanged && !_wrappedEntity.HasErrors),

                CancelCommand = new Command(() =>
                {
                    if (CloseView())
                        _closeAction?.Invoke();
                })
            };
        }

        protected virtual void InitializationComplete()
        {
            // initialize entity datacontext
            this.DataContext = _wrappedEntity.Entity;

            // mark as not changed forcibly
            _wrappedEntity.IsChanged = false;
            // add events
            _wrappedEntity.Entity.PropertyChanged += (sender, args) => ((Command)_commands.SaveCommand).ValidateCanExecute();
            // validate save command now 
            _commands.SaveCommand.ValidateCanExecute();
        }

        protected virtual void DetailsView_Loaded(object sender, RoutedEventArgs e)
        {
            // complete components initialization
            InitializationComplete();
            // set padding for content
            this.Padding = new Thickness(10);
        }

        #endregion

        #region Protected properties

        protected virtual string ViewHeader => _isNew
                ? MultiLangResourceManager.Instance[MultiLangResourceName.New(typeof(TEntity))]
                : MultiLangResourceManager.Instance[MultiLangResourceName.Singular(typeof(TEntity))];

        #endregion

        #region Public
        // TODO: add height and width
        public bool DialogResult { get; protected set; } = false;

        public virtual void SaveChanges()
        {
            if (_isNew)
                _service.Add(_wrappedEntity.Entity);
            else
                _service.Update(_wrappedEntity.Entity);

            DialogResult = true;
            _closeView = true;
        }

        public virtual void RevertChanges()
        {
            _wrappedEntity.RevertChanges();

            DialogResult = false;
            _closeView = true;
        }

        public virtual bool CloseView()
        {
            // not ask confirmation if it has already asked
            if (_closeView) return _closeView;

            // ask confirmation only if any changes exists
            if (_wrappedEntity.IsChanged)
            {
                // show confirmation
                var dialogResult = MessageBox.Show(MultiLangResourceManager.Instance[MultiLangResourceName.SaveChangesConfirmationMessage],
                    MultiLangResourceManager.Instance[MultiLangResourceName.SaveChangesConfirmation], MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Exclamation, MessageBoxResult.Yes);

                if (dialogResult == MessageBoxResult.Yes)
                {
                    // check errors
                    if (_wrappedEntity.HasErrors)
                        MessageBox.Show(MultiLangResourceManager.Instance[MultiLangResourceName.SaveFailedMessage],
                            MultiLangResourceManager.Instance[MultiLangResourceName.SaveFailed], MessageBoxButton.OK,
                            MessageBoxImage.Exclamation);
                    else
                        SaveChanges();
                }
                else if (dialogResult == MessageBoxResult.No)
                    RevertChanges();
            }
            else
                _closeView = true;

            return _closeView;
        }

        #endregion
    }
}
