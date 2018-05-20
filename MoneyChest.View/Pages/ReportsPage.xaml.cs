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
using LiveCharts.Definitions.Points;

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

        private ReportsPageViewModel<ChartViewModel> _viewModel;
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
            _viewModel = new ReportsPageViewModel<ChartViewModel>();

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

        #region Event handlers
        
        private void ReportGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChartLegend1.Height = ReportGrid.ActualHeight;
            ChartLegend2.Height = ReportGrid.ActualHeight;
            ChartLegend3.Height = ReportGrid.ActualHeight;
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

            // make sure curreny depth is correct
            if (!depthDictionary.ContainsKey(_viewModel.Settings.PieChartDetailsDepth))
                _viewModel.Settings.PieChartDetailsDepth = depthDictionary.Last().Key;

            comboDetailsDepth.ItemsSource = depthDictionary;
        }

        private void RebuildReport(bool force = false)
        {
            // hide all charts
            _viewModel.IsAnyData = false;

            // fill empty category name
            _builder.NoneCategoryName = MultiLangResourceManager.Instance[MultiLangResourceName.None];
            // build report result
            var result = _builder.Build(_viewModel.GetBuildSettings(), force);

            // cleanup
            if(_viewModel.ChartData.SeriesCollection.Count > 0)
                _viewModel.ChartData.SeriesCollection.Clear();
            _viewModel.ChartData.Titles.Clear();
            // cleanup chart data source
            pieChart.Series = null;
            barChartColumns.Series = null;
            barChartRows.Series = null;

            for (int i = 0; i < result.ReportUnits.Count; i++)
            {
                // build collection
                if(_viewModel.Settings.BarChartSection == BarChartSection.Category)
                {
                    _viewModel.ChartData.SeriesCollection.AddRange(BuildSeries(result.ReportUnits[i], i, result.ReportUnits.Count));
                }
                else
                {
                    if(_viewModel.ChartData.SeriesCollection.Count == 0)
                        _viewModel.ChartData.SeriesCollection.AddRange(BuildSeries(result.ReportUnits[i], i, result.ReportUnits.Count));

                    _viewModel.ChartData.SeriesCollection[0].Values[i] = new ObservableValue(result.ReportUnits[i].DoubleAmount);
                }
                // populate caption list
                _viewModel.ChartData.Titles.Add(result.ReportUnits[i].Caption);
                // mark any data exits
                _viewModel.IsAnyData = true;
            }

            // update total
            _viewModel.ChartData.Total = result.TotAmountDetailed;

            // populate chart datasource
            if (_viewModel.Settings.IsPieChartSelected) pieChart.Series = _viewModel.ChartData.SeriesCollection;
            if (_viewModel.Settings.IsBarChartColumnsSelected) barChartColumns.Series = _viewModel.ChartData.SeriesCollection;
            if (_viewModel.Settings.IsBarChartRowsSelected) barChartRows.Series = _viewModel.ChartData.SeriesCollection;
        }

        private IEnumerable<ISeriesView> BuildSeries(ReportUnit reportUnit, int itemIndex, int totalCount)
        {
            var result = new List<ISeriesView>();

            // series for pie chart
            if (_viewModel.Settings.IsPieChartSelected)
            {
                // values count should be equivalent to details depth (levels count)
                var valuesCount = _viewModel.Settings.PieChartDetailsDepth + 1;
                // add series for current report unit
                var series = BuildSeries<MCPieSeries>(reportUnit, 0, valuesCount);
                result.Add(series);
                // add all detailing report units as new series with next details depth (level)
                result.AddRange(BuildDetailsPieSeries(reportUnit, series, 1, valuesCount));
            }

            // series for bar chart with columns
            if (_viewModel.Settings.IsBarChartColumnsSelected)
            {
                if (reportUnit.Detailing.Count > 0)
                {
                    foreach(var reportUnitDetail in reportUnit.Detailing)
                        result.Add(BuildSeries<MCStackedColumnSeries>(reportUnitDetail, itemIndex, totalCount));
                }
                else
                    result.Add(BuildSeries<MCStackedColumnSeries>(reportUnit, itemIndex, totalCount));
            }

            // series for bar chart with rows
            if (_viewModel.Settings.IsBarChartRowsSelected)
            {
                if (reportUnit.Detailing.Count > 0)
                {
                    foreach (var reportUnitDetail in reportUnit.Detailing)
                        result.Add(BuildSeries<MCStackedRowSeries>(reportUnitDetail, itemIndex, totalCount));
                }
                else
                    result.Add(BuildSeries<MCStackedRowSeries>(reportUnit, itemIndex, totalCount));
            }

            return result;
        }

        private IEnumerable<ISeriesView> BuildDetailsPieSeries(ReportUnit parentReportUnit, ISeriesView parentSeries, 
            int valueIndex, int valuesCount)
        {
            var result = new List<ISeriesView>();

            foreach(var reportUnit in parentReportUnit.Detailing)
            {
                ISeriesView series = null;
                // if detailing report unit category is different create new series else continue parent series
                if(reportUnit.CategoryId != parentReportUnit.CategoryId)
                {
                    series = BuildSeries<MCPieSeries>(reportUnit, valueIndex, valuesCount);
                    result.Add(series);
                }
                else
                {
                    series = parentSeries;
                    series.Values[valueIndex] = new ObservableValue(reportUnit.DoubleAmount);
                }

                // build series for every detailed report unit as new level (next value index)
                if (reportUnit.Detailing.Count > 0)
                    result.AddRange(BuildDetailsPieSeries(reportUnit, series, valueIndex + 1, valuesCount));
            }

            return result;
        }

        private ISeriesView BuildSeries<TSeries>(ReportUnit reportUnit, int itemIndex, int itemsCount)
            where TSeries : Series, new()
        {
            var series = new TSeries()
            {
                Title = reportUnit.Caption,
                Values = new ChartValues<ObservableValue>(),
                DataLabels = true
            };

            // all previous and next values should be equal to 0 but not current 
            for (int i = 0; i < itemsCount; i++)
                series.Values.Add(new ObservableValue(i != itemIndex ? 0 : reportUnit.DoubleAmount));

            return series;
        }

        #endregion
    }
}
