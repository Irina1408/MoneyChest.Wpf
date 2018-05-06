using MahApps.Metro.IconPacks;
using MoneyChest.Model.Model;
using MoneyChest.Services;
using MoneyChest.Services.Services;
using MoneyChest.Services.Services.Settings;
using MoneyChest.Shared;
using MoneyChest.Shared.MultiLang;
using MoneyChest.ViewModel.ViewModel;
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

namespace MoneyChest.View.Pages
{
    /// <summary>
    /// Interaction logic for ReportsPage.xaml
    /// </summary>
    public partial class ReportsPage : PageBase
    {
        #region Private fields

        private ITransactionService _service;
        private ICurrencyService _currencyService;
        private IStorageService _storageService;
        private ICurrencyExchangeRateService _currencyExchangeRateService;
        private IReportSettingService _settingsService;

        private ReportsPageViewModel _viewModel;

        #endregion

        #region Initialization

        public ReportsPage() : base()
        {
            InitializeComponent();

            // init
            _service = ServiceManager.ConfigureService<TransactionService>();
            _currencyService = ServiceManager.ConfigureService<CurrencyService>();
            _storageService = ServiceManager.ConfigureService<StorageService>();
            _currencyExchangeRateService = ServiceManager.ConfigureService<CurrencyExchangeRateService>();
            _settingsService = ServiceManager.ConfigureService<ReportSettingService>();

            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            _viewModel = new ReportsPageViewModel();

            this.DataContext = _viewModel;
        }

        #endregion

        #region Overrides

        public override void Reload()
        {
            base.Reload();

            if (_viewModel.Settings == null)
            {
                _viewModel.Settings = _settingsService.GetForUser(GlobalVariables.UserId);

                _viewModel.Settings.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName != nameof(ReportSettingModel.DataFilter) ||
                        e.PropertyName == nameof(ReportSettingModel.PeriodFilter))
                    {
                        // save changes
                        _settingsService.Update(_viewModel.Settings);
                        // apply settings
                        //ApplySettings();
                    }
                };

                // add notifications for reload and apply filter
                _viewModel.Settings.PeriodFilter.OnPeriodChanged += (sender, e) =>
                {
                    // save changes
                    _settingsService.Update(_viewModel.Settings);
                    // reload page
                    Reload();
                };

                _viewModel.Settings.DataFilter.OnFilterChanged += (sender, e) =>
                {
                    // save changes
                    _settingsService.Update(_viewModel.Settings);
                    // apply filter
                    //ApplyDataFilter();
                };
            }
        }

        #endregion
    }
}
