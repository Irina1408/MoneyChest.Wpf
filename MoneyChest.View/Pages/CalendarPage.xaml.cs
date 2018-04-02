using MahApps.Metro.IconPacks;
using MoneyChest.Calculation.Builders;
using MoneyChest.Model.Enums;
using MoneyChest.Services;
using MoneyChest.Services.Services;
using MoneyChest.Shared;
using MoneyChest.Shared.MultiLang;
using MoneyChest.View.Components;
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
    /// Interaction logic for CalendarPage.xaml
    /// </summary>
    public partial class CalendarPage : PageBase
    {
        #region Private fields

        private ITransactionService _service;
        private ICalendarSettingsService _settingsService;
        private CalendarPageViewModel _viewModel;
        private CalendarDataBuilder _builder;
        private List<CellMapping> _cellMapping;
        private PeriodType? _lastPeriodType;
        private List<DayOfWeek> _daysOfWeek;

        #endregion

        #region Initialization

        public CalendarPage() : base()
        {
            InitializeComponent();

            // init
            _service = ServiceManager.ConfigureService<TransactionService>();
            _settingsService = ServiceManager.ConfigureService<CalendarSettingsService>();
            _builder = new CalendarDataBuilder(GlobalVariables.UserId, _service);
            _cellMapping = new List<CellMapping>();
            _daysOfWeek = new List<DayOfWeek>();

            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            _viewModel = new CalendarPageViewModel();

            this.DataContext = _viewModel;
        }

        #endregion

        #region Overrides

        public override void Reload()
        {
            base.Reload();

            if(_viewModel.Settings == null)
            {
                _viewModel.Settings = _settingsService.GetForUser(GlobalVariables.UserId);

                // add notifications for reload and apply filter
                _viewModel.Settings.PeriodFilter.OnPeriodChanged += (sender, e) =>
                {
                    // save changes
                    _settingsService.Update(_viewModel.Settings);
                    // reload page
                    Reload();
                };
                
                _viewModel.Settings.DataFilter.PropertyChanged += (sender, e) =>
                {
                    // save changes
                    _settingsService.Update(_viewModel.Settings);
                    // apply filter
                    //ApplyDataFilter();
                };
            }

            // reload calendar data and update view
            RefreshCalendar();

            // apply filter
            //ApplyDataFilter();
        }

        #endregion

        #region Private methods

        private void RefreshCalendar()
        {
            // build data
            var data = _builder.Build(_viewModel.Settings.PeriodFilter.DateFrom, _viewModel.Settings.PeriodFilter.DateUntil);

            // fill headers
            FeelHeaders();

            // clean up when period type wasn't defined or was changed
            if (_lastPeriodType != _viewModel.Settings.PeriodFilter.PeriodType)
            {
                daysGrid.Children.Clear();
                daysGrid.Rows = 1;
                _lastPeriodType = _viewModel.Settings.PeriodFilter.PeriodType;
            }

            // temporary data
            var iRow = 0;
            var iCol = 0;
            var currDate = _viewModel.Settings.PeriodFilter.DateFrom.Date;

            // populate empty days before start of selected period
            for(iCol = 0; iCol < _daysOfWeek.Count; iCol++)
            {
                if (_daysOfWeek[iCol] != _viewModel.Settings.PeriodFilter.DateFrom.DayOfWeek) SetDay(iCol, iRow, null);
                else break;
            }

            // next row if first day of selected period is the last day of week
            //if (iCol == _daysOfWeek.Count - 1) iRow += 1;

            // fill selected period days
            foreach(var d in data)
            {
                SetDay(iCol, iRow, d);

                if (iCol == _daysOfWeek.Count - 1)
                {
                    iRow += 1;
                    iCol = 0;
                }
                else
                    iCol += 1;
            }

            // populate empty days after selected period
            for (; iCol > 0 && iCol < _daysOfWeek.Count; iCol++)
            {
                SetDay(iCol, iRow, null);
            }
        }

        private void FeelHeaders()
        {
            if (_daysOfWeek.Count == 0)
            {
                // build correct sequence of days of week
                var iCurrNext = 0;
                foreach (DayOfWeek d in Enum.GetValues(typeof(DayOfWeek)))
                {
                    // insert into the start first day of week
                    if (d == GlobalVariables.FirstDayOfWeek)
                        _daysOfWeek.Insert(iCurrNext++, d);

                    // when first day of week haven't been found add next day
                    else if (iCurrNext == 0)
                        _daysOfWeek.Add(d);

                    // when first day of week have been found insert next day after first day 
                    else
                        _daysOfWeek.Insert(iCurrNext++, d);
                }

                for (int iCol = 0; iCol < 7; iCol++)
                {
                    var lbl = new Label()
                    {
                        Content = MultiLangResource.EnumItemDescription(typeof(DayOfWeek), _daysOfWeek[iCol])
                    };

                    Grid.SetColumn(lbl, iCol);
                    dayHeadersGrid.Children.Add(lbl);
                }
            }
        }

        private void SetDay(int iCol, int iRow, CalendarDayData data)
        {
            var cellMap = _cellMapping.FirstOrDefault(x => x.ICol == iCol && x.IRow == iRow);

            if (cellMap == null)
            {
                // create new
                cellMap = new CellMapping()
                {
                    ICol = iCol,
                    IRow = iRow,
                    Control = new CalendarDayControl()
                };
                // add to the list
                _cellMapping.Add(cellMap);

                // make sure grid contains correspond row
                if (daysGrid.Rows < iRow + 1)
                    daysGrid.Rows = iRow + 1;
                // fill cell in grid
                Grid.SetColumn(cellMap.Control, cellMap.ICol);
                Grid.SetRow(cellMap.Control, cellMap.IRow);
                daysGrid.Children.Add(cellMap.Control);
            }
            
            if(cellMap.Control.Parent == null)
                daysGrid.Children.Add(cellMap.Control);

            cellMap.Control.Data = data;
        }

        #endregion

        #region Helper structures

        private class CellMapping
        {
            public int ICol { get; set; }
            public int IRow { get; set; }
            public CalendarDayControl Control { get; set; }

        }

        #endregion
    }
}
