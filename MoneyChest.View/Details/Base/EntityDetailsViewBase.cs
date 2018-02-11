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
    public interface IEntityDetailsView
    {
        void PrepareParentWindow(Window window);
        bool DialogResult { get; }
    }

    public abstract class EntityDetailsViewBase<TEntity, TViewModel, TService> : UserControl, IEntityDetailsView
        where TEntity : class, INotifyPropertyChanged
        where TViewModel : class, TEntity, INotifyPropertyChanged
        where TService : IServiceBase<TEntity>
    {
        #region Initialization
        
        public EntityDetailsViewBase() : base()
        { }

        public EntityDetailsViewBase(TService service, TViewModel entity, bool isNew) : this()
        {
            // init
            Service = service;
            IsNew = isNew;
            WrappedEntity = new EntityWrapper<TViewModel>(entity);

            // init commands
            InitializeCommands();
        }

        protected virtual void InitializeCommands()
        {
            Commands = new DetailsViewCommandContainer()
            {
                SaveCommand = new Command(() =>
                {
                    // save changes
                    SaveChanges();
                    // close control
                    Close(false);
                },
                () => WrappedEntity.IsChanged && !WrappedEntity.HasErrors),

                CancelCommand = new Command(() => Close(true))
            };
        }

        protected virtual void InitializationComplete()
        {
            // initialize entity datacontext
            this.DataContext = WrappedEntity.Entity;

            // mark as not changed forcibly
            WrappedEntity.IsChanged = false;
            // add events
            WrappedEntity.Entity.PropertyChanged += (sender, args) => ((Command)Commands.SaveCommand).ValidateCanExecute();
            // validate save command now 
            Commands.SaveCommand.ValidateCanExecute();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            
            // complete components initialization
            InitializationComplete();
            // set padding for content
            this.Padding = new Thickness(10);
        }

        #endregion

        #region Protected properties & methods

        protected virtual string ViewHeader => IsNew
                ? MultiLangResourceManager.Instance[MultiLangResourceName.New(typeof(TEntity))]
                : MultiLangResourceManager.Instance[MultiLangResourceName.Singular(typeof(TEntity))];

        protected TService Service { get; private set; }
        protected EntityWrapper<TViewModel> WrappedEntity { get; private set; }
        protected DetailsViewCommandContainer Commands { get; private set; }
        protected bool IsNew { get; private set; }

        protected virtual void SaveChanges()
        {
            if (IsNew)
                Service.Add(WrappedEntity.Entity);
            else
                Service.Update(WrappedEntity.Entity);

            WrappedEntity.IsChanged = false;
            DialogResult = true;
        }

        protected virtual void RevertChanges()
        {
            WrappedEntity.RevertChanges();

            WrappedEntity.IsChanged = false;
            DialogResult = false;
        }

        #endregion

        #region IBaseEntityDetailsView implementation

        public virtual void PrepareParentWindow(Window window)
        {
            window.Closing += Window_Closing;
        }

        public bool DialogResult { get; protected set; } = false;

        #endregion

        #region Private methods

        private bool Close(bool askConfirmation, bool inner = false)
        {
            bool close = !askConfirmation || !WrappedEntity.IsChanged;

            // ask confirmation only if any changes exists
            if (askConfirmation && WrappedEntity.IsChanged)
            {
                // show confirmation
                var dialogResult = MessageBox.Show(MultiLangResourceManager.Instance[MultiLangResourceName.SaveChangesConfirmationMessage],
                    MultiLangResourceManager.Instance[MultiLangResourceName.SaveChangesConfirmation], MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Exclamation, MessageBoxResult.Yes);

                if (dialogResult == MessageBoxResult.Yes)
                {
                    // check errors
                    if (WrappedEntity.HasErrors)
                        MessageBox.Show(MultiLangResourceManager.Instance[MultiLangResourceName.SaveFailedMessage],
                            MultiLangResourceManager.Instance[MultiLangResourceName.SaveFailed], MessageBoxButton.OK,
                            MessageBoxImage.Exclamation);
                    else
                    {
                        SaveChanges();
                        close = true;
                    }
                }
                else if (dialogResult == MessageBoxResult.No)
                {
                    RevertChanges();
                    close = true;
                }
            }

            if (!inner && close)
                CloseWindow();

            return close;
        }

        private void CloseWindow()
        {
            var window = Window.GetWindow(this);
            if (window == null) return;

            // detach event
            window.Closing -= Window_Closing;
            // close window
            window.Close();
            // attach event
            window.Closing += Window_Closing;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!Close(true, true))
                e.Cancel = true;
        }

        #endregion
    }
}
