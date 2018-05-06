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
using LiveCharts;
using LiveCharts.Wpf;
using MoneyChest.Calculation.Builders;
using LiveCharts.Defaults;
using System.ComponentModel;

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
        private ICategoryService _categoryService;
        private IReportSettingService _settingsService;
        private ReportDataBuilder _builder;

        private ReportsPageViewModel<ChartSpecial> _viewModel;

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
            _categoryService = ServiceManager.ConfigureService<CategoryService>();
            _settingsService = ServiceManager.ConfigureService<ReportSettingService>();
            _builder = new ReportDataBuilder(GlobalVariables.UserId, _service, _currencyService, _currencyExchangeRateService, _categoryService);

            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            _viewModel = new ReportsPageViewModel<ChartSpecial>();

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
                if (_viewModel.Settings.DataType == null)
                    _viewModel.Settings.DataType = Model.Enums.RecordType.Expense;

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

            // build report result
            var result = _builder.Build(_viewModel.Settings.PeriodFilter.DateFrom, _viewModel.Settings.PeriodFilter.DateUntil, _viewModel.Settings.CategoryLevel, _viewModel.Settings.DataType.Value, true);

            _viewModel.Special.PieCollection.Clear();
            _viewModel.Special.Titles.Clear();
            _viewModel.Special.BarColumnCollection.Clear();
            _viewModel.Special.BarRowCollection.Clear();

            var nonCategoryName = MultiLangResourceManager.Instance[MultiLangResourceName.None];

            for(int i = 0; i < result.ReportUnits.Count; i++)
            {
                var value = Convert.ToDouble(result.ReportUnits[i].Amount);
                var caption = result.ReportUnits[i].Caption ?? nonCategoryName;

                // pie chart collection
                _viewModel.Special.PieCollection.Add(new PieSeries()
                {
                    Title = caption,
                    Values = new ChartValues<ObservableValue>() { new ObservableValue(value) }
                });

                _viewModel.Special.DoughnutCollection.Add(new PieSeries()
                {
                    Title = caption,
                    Values = new ChartValues<ObservableValue>() { new ObservableValue(value) }
                });

                // bar chart (columns) collection
                //_viewModel.Special.Titles.Add(caption);
                // build column series for current bar
                var columnSeries = new StackedColumnSeries
                {
                    Title = caption,
                    Values = new ChartValues<ObservableValue>()
                };
                // all previous and next values should be equal to 0 but not current 
                for (int iVal = 0; iVal < result.ReportUnits.Count; iVal++)
                    columnSeries.Values.Add(new ObservableValue(iVal != i ? 0 : value));
                // update bar chart collection
                _viewModel.Special.BarColumnCollection.Add(columnSeries);

                // bar chart (row) collection
                //_viewModel.Special.Titles.Add(caption);
                // build column series for current bar
                var rowSeries = new StackedRowSeries
                {
                    Title = caption,
                    Values = new ChartValues<ObservableValue>()
                };
                // all previous and next values should be equal to 0 but not current 
                for (int iVal = 0; iVal < result.ReportUnits.Count; iVal++)
                    rowSeries.Values.Add(new ObservableValue(iVal != i ? 0 : value));
                // update bar chart collection
                _viewModel.Special.BarRowCollection.Add(rowSeries);
            }

            // update total
            _viewModel.Total = result.TotAmountDetailed;
        }

        #endregion
    }

    public class ChartSpecial
    {
        public SeriesCollection PieCollection { get; set; } = new SeriesCollection();
        public SeriesCollection DoughnutCollection { get; set; } = new SeriesCollection();
        public SeriesCollection BarColumnCollection { get; set; } = new SeriesCollection();
        public SeriesCollection BarRowCollection { get; set; } = new SeriesCollection();
        public List<string> Titles { get; set; } = new List<string>();
    }
}
