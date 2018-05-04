using MoneyChest.Model.Enums;
using MoneyChest.Model.Model;
using MoneyChest.Services;
using MoneyChest.Services.Services.Settings;
using MoneyChest.Shared;
using MoneyChest.Shared.MultiLang;
using MoneyChest.Shared.Settings;
using MoneyChest.View.Utils;
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
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : PageBase
    {
        #region Private fields

        private IGeneralSettingService _service;
        private SettingsPageViewModel _viewModel;

        #endregion

        #region Initialization

        public SettingsPage() : base()
        {
            InitializeComponent();

            // init
            _service = ServiceManager.ConfigureService<GeneralSettingService>();
            _viewModel = new SettingsPageViewModel();
            comboLanguages.ItemsSource = MultiLangEnumHelper.ToCollection(typeof(Language));
            comboAccentColors.ItemsSource = MCThemeManager.Instance.AccentColors;
            comboThemeColors.ItemsSource = MCThemeManager.Instance.ThemeColors;

            this.DataContext = _viewModel;
        }

        #endregion

        #region Overrides

        public override bool IsOptionsPage => true;

        public override void Reload()
        {
            base.Reload();

            if(_viewModel.Settings == null)
            {
                _viewModel.Settings = _service.GetForUser(GlobalVariables.UserId);

                _viewModel.Settings.PropertyChanged += (sender, e) =>
                {
                    // set new language
                    if(e.PropertyName == nameof(GeneralSettingModel.Language))
                    {
                        MultiLangResourceManager.Instance.SetLanguage(_viewModel.Settings.Language);
                        AppSettings.Instance.LastLanguage = _viewModel.Settings.Language;
                        AppSettings.Instance.Save();
                    }

                    // set new accent color
                    if (e.PropertyName == nameof(GeneralSettingModel.AccentColor))
                    {
                        MCThemeManager.Instance.SetAccentColor(_viewModel.Settings.AccentColor);
                        AppSettings.Instance.LastAccentColor = _viewModel.Settings.AccentColor;
                        AppSettings.Instance.Save();
                    }

                    // set new theme color
                    if (e.PropertyName == nameof(GeneralSettingModel.ThemeColor))
                    {
                        MCThemeManager.Instance.SetThemeColor(_viewModel.Settings.ThemeColor);
                        AppSettings.Instance.LastThemeColor = _viewModel.Settings.ThemeColor;
                        AppSettings.Instance.Save();
                    }

                    // save changes
                    _service.Update(_viewModel.Settings);
                };
            }
        }

        #endregion

        #region Private methods

        

        #endregion
    }
}
