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
using LiveCharts.Configurations;
using MoneyChest.View.Utils;

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
        private ChartDataBuilder _chartDataBuilder;

        private ReportsPageViewModel<ChartData> _viewModel;
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
            _chartDataBuilder = new ChartDataBuilder();

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
            _viewModel = new ReportsPageViewModel<ChartData>();

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

                // settings that should be saved but rebuild isn't required
                var saveSettings = new List<string>() {
                    nameof(ReportSettingModel.PieChartInnerRadius),
                    nameof(ReportSettingModel.ShowSettings) };

                // settings that requires data reload for report
                var requiresReloadSettings = new List<string>() {
                    nameof(ReportSettingModel.IncludeActualTransactions),
                    nameof(ReportSettingModel.IncludeFuturePlannedTransactions) };

                // save and rebuild report handler
                void buildSettingsChanged (bool requiresDataReload = false)
                {
                    // save changes
                    _settingsService.Update(_viewModel.Settings);
                    // reload report data
                    RebuildReport(requiresDataReload);
                };

                _viewModel.Settings.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName != nameof(ReportSettingModel.DataFilter) &&
                        e.PropertyName != nameof(ReportSettingModel.PeriodFilter) &&
                        typeof(ReportSettingModel).GetProperty(e.PropertyName).CanWrite)
                    {
                        if(saveSettings.Contains(e.PropertyName))
                        {
                            // save changes
                            _settingsService.Update(_viewModel.Settings);
                        }
                        else if(e.PropertyName == nameof(ReportSettingModel.ShowLegend))
                        {
                            // update legend visibility
                            UpdateLegendVisibility(_viewModel.Settings.ShowLegendView);
                            // save changes
                            _settingsService.Update(_viewModel.Settings);
                        }
                        else if (e.PropertyName == nameof(ReportSettingModel.ShowValue))
                        {
                            // update labels visibility
                            _chartDataBuilder.UpdateShowLables(_viewModel.ChartData.SeriesCollection, _viewModel.Settings.ShowValue);
                            // save changes
                            _settingsService.Update(_viewModel.Settings);
                        }
                        else
                        {
                            if (e.PropertyName == nameof(ReportSettingModel.CategoryLevel))
                            {
                                // refresh items source of comboDetailsDepth
                                RefreshAvailableDetailsDepth();
                            }

                            // rebuild report and save settings
                            buildSettingsChanged(requiresReloadSettings.Contains(e.PropertyName));
                        }
                    }
                };

                // add notifications for reload and apply filter
                _viewModel.Settings.PeriodFilter.OnPeriodChanged += (sender, e) => buildSettingsChanged();
                _viewModel.Settings.DataFilter.OnFilterChanged += (sender, e) =>
                {
                    if (e.PropertyName == nameof(DataFilterModel.IsFilterApplied) && !_viewModel.Settings.DataFilter.IsDataFiltered
                        || e.PropertyName == nameof(DataFilterModel.IsFilterVisible))
                        _settingsService.Update(_viewModel.Settings);
                    else
                        buildSettingsChanged();
                };

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
            using (new WaitCursor())
            {
                // hide all charts
                _viewModel.IsAnyData = false;

                // fill empty category name
                _builder.NoneCategoryName = MultiLangResourceManager.Instance[MultiLangResourceName.None];
                // build report result
                var result = _builder.Build(_viewModel.GetBuildSettings(), force);

                // cleanup chart data source
                pieChart.Series = null;
                barChartColumns.Series = null;
                barChartRows.Series = null;

                _viewModel.ChartData = _chartDataBuilder.Build(result.ReportUnits, _viewModel.Settings);
                _viewModel.IsAnyData = _viewModel.ChartData.SeriesCollection.Any();
                // update total
                _viewModel.ChartData.Total = result.TotAmountDetailed;

                // populate chart datasource
                if (_viewModel.Settings.IsPieChartSelected) pieChart.Series = _viewModel.ChartData.SeriesCollection;
                if (_viewModel.Settings.IsBarChartColumnsSelected) barChartColumns.Series = _viewModel.ChartData.SeriesCollection;
                if (_viewModel.Settings.IsBarChartRowsSelected) barChartRows.Series = _viewModel.ChartData.SeriesCollection;
                // update legend visibility
                UpdateLegendVisibility(_viewModel.Settings.ShowLegendView);
            }
        }

        private void UpdateLegendVisibility(bool showLegend)
        {
            pieChart.LegendLocation = showLegend ? LegendLocation.Right : LegendLocation.None;
            barChartColumns.LegendLocation = showLegend ? LegendLocation.Right : LegendLocation.None;
            barChartRows.LegendLocation = showLegend ? LegendLocation.Right : LegendLocation.None;
        }

        #endregion
    }
}
