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
using MoneyChest.Model.Enums;
using LiveCharts.Definitions.Series;
using MoneyChest.Model.Report;

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
        private int lowestCategoryLevel;

        #endregion

        #region Initialization

        public ReportsPage() : base()
        {
            InitializeComponent();
        }

        protected override void InitializationComplete()
        {
            // init
            _service = ServiceManager.ConfigureService<TransactionService>();
            _currencyService = ServiceManager.ConfigureService<CurrencyService>();
            _storageService = ServiceManager.ConfigureService<StorageService>();
            _currencyExchangeRateService = ServiceManager.ConfigureService<CurrencyExchangeRateService>();
            _categoryService = ServiceManager.ConfigureService<CategoryService>();
            _settingsService = ServiceManager.ConfigureService<ReportSettingService>();
            _builder = new ReportDataBuilder(GlobalVariables.UserId, _service, _currencyService, _currencyExchangeRateService, _categoryService);

            // init comboboxes
            comboChartType.ItemsSource = MultiLangEnumHelper.ToCollection(typeof(ChartType));
            comboDataType.ItemsSource = MultiLangEnumHelper.ToCollection(typeof(RecordType));
            comboBarChartView.ItemsSource = MultiLangEnumHelper.ToCollection(typeof(BarChartView));
            comboSorting.ItemsSource = MultiLangEnumHelper.ToCollection(typeof(Sorting));
            comboBarChartSection.ItemsSource = MultiLangEnumHelper.ToCollection(typeof(BarChartSection));

            // fill PeriodTypes. exclude custom period
            var values = new List<object>();
            foreach (PeriodType enumItem in Enum.GetValues(typeof(PeriodType)))
                if(enumItem != PeriodType.Custom) values.Add(enumItem);
            comboBarChartPeriod.ItemsSource = MultiLangEnumHelper.ToCollection(typeof(PeriodType), values);

            // fill category levels source
            lowestCategoryLevel = _categoryService.GetLowestCategoryLevel(GlobalVariables.UserId);
            var categoryLevelDictionary = new Dictionary<int, string>();
            // add variant "All"
            categoryLevelDictionary.Add(-1, MultiLangResourceManager.Instance[MultiLangResourceName.All]);
            for (int i = 0; i <= lowestCategoryLevel; i++)
                categoryLevelDictionary.Add(i, (i + 1).ToString());
            comboCategoryLevel.ItemsSource = categoryLevelDictionary;

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

                EventHandler buildSettingsChangedHandler = (sender, e) =>
                {
                    // save changes
                    _settingsService.Update(_viewModel.Settings);
                    // reload report data
                    RebuildReport();
                };

                _viewModel.Settings.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName != nameof(ReportSettingModel.DataFilter) ||
                        e.PropertyName != nameof(ReportSettingModel.PeriodFilter))
                    {
                        // TODO: do not rebuild report data when view settings are changed (cart/inner radius etc..)
                        buildSettingsChangedHandler.Invoke(sender, e);

                        if (e.PropertyName == nameof(ReportSettingModel.CategoryLevel))
                        {
                            // refresh items source of comboDetailsDepth
                            RefreshAvailableDetailsDepth();
                        }
                    }
                };

                // add notifications for reload and apply filter
                _viewModel.Settings.PeriodFilter.OnPeriodChanged += buildSettingsChangedHandler;
                _viewModel.Settings.DataFilter.OnFilterChanged += buildSettingsChangedHandler;

                // refresh items source of comboDetailsDepth
                RefreshAvailableDetailsDepth();
            }

            RebuildReport(true);
        }

        #endregion

        #region Private methods

        private void RefreshAvailableDetailsDepth()
        {
            var depthDictionary = new Dictionary<int, int>();
            if (_viewModel.Settings.CategoryLevel != -1)
            {
                for (int i = 0, lvl = _viewModel.Settings.CategoryLevel; lvl <= lowestCategoryLevel; i++, lvl++)
                    depthDictionary.Add(i, i + 1);
            }
            else
                depthDictionary.Add(0, 1);

            comboDetailsDepth.ItemsSource = depthDictionary;
        }

        private void RebuildReport(bool force = false)
        {
            // hide all charts
            _viewModel.IsAnyData = false;

            // build report result
            var result = _builder.Build(_viewModel.GetBuildSettings(), force);

            // cleanup
            if(_viewModel.Special.SeriesCollection.Count > 0)
                _viewModel.Special.SeriesCollection.Clear();
            _viewModel.Special.Titles.Clear();
            // cleanup chart data source
            pieChart.Series = null;
            barChartColumns.Series = null;
            barChartRows.Series = null;

            var nonCategoryName = MultiLangResourceManager.Instance[MultiLangResourceName.None];

            for (int i = 0; i < result.ReportUnits.Count; i++)
            {
                var value = Convert.ToDouble(result.ReportUnits[i].Amount);
                var caption = result.ReportUnits[i].Caption ?? nonCategoryName;

                // build collection
                _viewModel.Special.SeriesCollection.Add(BuildSeries(caption, value, i, result.ReportUnits.Count));
                // populate caption list
                _viewModel.Special.Titles.Add(caption);
                // mark any data exits
                _viewModel.IsAnyData = true;
            }

            // update total
            _viewModel.Total = result.TotAmountDetailed;

            // populate chart datasource
            if (_viewModel.Settings.IsPieChartSelected) pieChart.Series = _viewModel.Special.SeriesCollection;
            if (_viewModel.Settings.IsBarChartColumnsSelected) barChartColumns.Series = _viewModel.Special.SeriesCollection;
            if (_viewModel.Settings.IsBarChartRowsSelected) barChartRows.Series = _viewModel.Special.SeriesCollection;
        }

        private ISeriesView BuildSeries(string caption, double value, int itemIndex, int itemCount)
        {
            if (_viewModel.Settings.IsPieChartSelected)
            {
                // series for pie chart
                return new PieSeries()
                {
                    Title = caption,
                    Values = new ChartValues<ObservableValue>() { new ObservableValue(value) },
                    DataLabels = true
                };
            }
            if (_viewModel.Settings.IsBarChartColumnsSelected)
            {
                // series for bar chart with columns
                var columnSeries = new StackedColumnSeries
                {
                    Title = caption,
                    Values = new ChartValues<ObservableValue>(),
                    DataLabels = true
                };
                // all previous and next values should be equal to 0 but not current 
                for (int i = 0; i < itemCount; i++)
                    columnSeries.Values.Add(new ObservableValue(i != itemIndex ? 0 : value));

                return columnSeries;
            }
            if (_viewModel.Settings.IsBarChartRowsSelected)
            {
                // series for bar chart with rows
                var rowSeries = new StackedRowSeries
                {
                    Title = caption,
                    Values = new ChartValues<ObservableValue>(),
                    DataLabels = true
                };
                // all previous and next values should be equal to 0 but not current 
                for (int i = 0; i < itemCount; i++)
                    rowSeries.Values.Add(new ObservableValue(i != itemIndex ? 0 : value));

                return rowSeries;
            }

            return null;
        }

        #endregion
    }

    public class ChartSpecial
    {
        public SeriesCollection SeriesCollection { get; set; } = new SeriesCollection();
        public List<string> Titles { get; set; } = new List<string>();
    }
}
