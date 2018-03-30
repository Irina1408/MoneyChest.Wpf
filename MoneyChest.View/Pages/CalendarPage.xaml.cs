using MahApps.Metro.IconPacks;
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

        private CalendarPageViewModel _viewModel;
        private ITransactionService _service;
        private ICalendarSettingsService _settingsService;

        #endregion

        #region Initialization

        public CalendarPage() : base()
        {
            InitializeComponent();

            // init
            _service = ServiceManager.ConfigureService<TransactionService>();
            _settingsService = ServiceManager.ConfigureService<CalendarSettingsService>();

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


            // apply filter
            //ApplyDataFilter();
        }

        #endregion
    }
}
