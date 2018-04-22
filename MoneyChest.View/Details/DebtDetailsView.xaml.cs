using MaterialDesignThemes.Wpf;
using MoneyChest.Model.Extensions;
using MoneyChest.Model.Model;
using MoneyChest.Services;
using MoneyChest.Services.Services;
using MoneyChest.Services.Services.Base;
using MoneyChest.Shared;
using MoneyChest.Shared.MultiLang;
using MoneyChest.View.Utils;
using MoneyChest.ViewModel.Commands;
using MoneyChest.ViewModel.ViewModel;
using MoneyChest.ViewModel.Wrappers;
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
    public abstract class DebtDetailsViewBase : EntityDetailsViewBase<DebtModel, DebtViewModel, IDebtService>
    {
        public DebtDetailsViewBase() : base()
        { }

        public DebtDetailsViewBase(IDebtService service, DebtViewModel entity, bool isNew) : base(service, entity, isNew)
        { }
    }

    /// <summary>
    /// Interaction logic for DebtDetailsView.xaml
    /// </summary>
    public partial class DebtDetailsView : DebtDetailsViewBase
    {
        #region Private fields
        
        private IEnumerable<CurrencyModel> _currencies;
        private IEnumerable<StorageModel> _storages;
        private ICommand DeletePenaltyCommand;

        private Dictionary<DebtPenaltyViewModel, ContentControl> _penaltyControl;

        #endregion

        #region Initialization

        public DebtDetailsView(IDebtService service, DebtViewModel entity, bool isNew) : base(service, entity, isNew)
        {
            InitializeComponent();
            
            // load storages
            IStorageService storageService = ServiceManager.ConfigureService<StorageService>();
            _storages = storageService.GetVisible(GlobalVariables.UserId, entity.StorageId ?? 0);

            // add empty storage
            var storages = new List<StorageModel>();
            storages.Add(new StorageModel() { Id = -1, Name = MultiLangResourceManager.Instance[MultiLangResourceName.None] });
            storages.AddRange(_storages);
            comboStorage.ItemsSource = storages;
            if (IsNew)
                entity.StorageId = -1;

            // load currencies
            ICurrencyService currencyService = ServiceManager.ConfigureService<CurrencyService>();
            _currencies = currencyService.GetActive(GlobalVariables.UserId, entity.CurrencyId, entity.Storage?.CurrencyId);
            comboCurrencies.ItemsSource = _currencies;
            
            FillPenalties();

            // set currencies list
            compCurrencyExchangeRate.CurrencyIds = _storages.Select(_ => _.CurrencyId).Concat(_currencies.Select(c => c.Id))
                .Distinct().ToList();

            // set header and commands panel context
            LabelHeader.Content = ViewHeader;
            CommandsPanel.DataContext = Commands;
        }

        protected override void InitializeCommands()
        {
            base.InitializeCommands();

            DeletePenaltyCommand = new ParametrizedCommand<DebtPenaltyViewModel>(
                (item) =>
                {
                    var message = MultiLangResource.DeletionConfirmationMessage(typeof(DebtPenaltyModel), new[] { item.Description });

                    if (MessageBox.Show(message, MultiLangResourceManager.Instance[MultiLangResourceName.DeletionConfirmation],
                        MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                    {
                        // remove from entity
                        WrappedEntity.Entity.Penalties.Remove(WrappedEntity.Entity.Penalties.First(_ => _.Id == item.Id));
                        // remove from view
                        PenaltiesPanel.Children.Remove(_penaltyControl[item]);
                    }
                });

            WrappedEntity.Entity.AddPenaltyCommand = new Command(() =>
            {
                var newPenalty = new DebtPenaltyModel()
                {
                    DebtId = WrappedEntity.Entity.Id,
                    Id = WrappedEntity.Entity.Penalties.Count > 0 ?WrappedEntity.Entity.Penalties.Min(_ => _.Id) -1 : -1
                };

                WrappedEntity.Entity.Penalties.Add(newPenalty);
                AddPenaltyToView(newPenalty);
            });
        }

        public override void PrepareParentWindow(Window window)
        {
            base.PrepareParentWindow(window);

            window.Height = 572;
            window.Width = 1090;
        }

        #endregion

        #region Event handlers

        private void comboStorage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!WrappedEntity.IsChanged) return;
            // update storage currency
            WrappedEntity.Entity.Storage = _storages.FirstOrDefault(_ => _.Id == WrappedEntity.Entity.StorageId)?.ToReferenceView();
        }

        private void comboCurrencies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!WrappedEntity.IsChanged) return;
            // update debt currency
            WrappedEntity.Entity.Currency = _currencies.FirstOrDefault(_ => _.Id == WrappedEntity.Entity.CurrencyId)?.ToReferenceView();
        }

        #endregion

        #region Private methods

        private void FillPenalties()
        {
            _penaltyControl = new Dictionary<DebtPenaltyViewModel, ContentControl>();

            foreach (var penalty in WrappedEntity.Entity.Penalties)
                AddPenaltyToView(penalty);
        }

        private void AddPenaltyToView(DebtPenaltyModel penalty)
        {
            // TODO: should be updated the same penalty object
            var penaltyViewModel = new DebtPenaltyViewModel(penalty)
            {
                DeleteCommand = DeletePenaltyCommand
            };

            var penaltyControl = new ContentControl()
            {
                Template = this.Resources["PenaltyItemControlTemplate"] as ControlTemplate,
                DataContext = penaltyViewModel
            };

            _penaltyControl.Add(penaltyViewModel, penaltyControl);
            PenaltiesPanel.Children.Add(penaltyControl);
        }

        #endregion
    }
}
