using MahApps.Metro.IconPacks;
using MoneyChest.Calculation.Builders;
using MoneyChest.Services;
using MoneyChest.Services.Services;
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
    /// Interaction logic for CalendarPage.xaml
    /// </summary>
    public partial class CalendarPage : PageBase
    {
        #region Private fields

        private ITransactionService _service;
        private ICalendarSettingsService _settingsService;
        private CalendarPageViewModel _viewModel;
        private CalendarDataBuilder _builder;

        #endregion

        #region Initialization

        public CalendarPage() : base()
        {
            InitializeComponent();

            // init
            _service = ServiceManager.ConfigureService<TransactionService>();
            _settingsService = ServiceManager.ConfigureService<CalendarSettingsService>();
            _builder = new CalendarDataBuilder(GlobalVariables.UserId, _service);

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

            for(int iCol = 0; iCol < 7; iCol++)
            {

            }
        }

        #endregion
    }
}
