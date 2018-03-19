using MahApps.Metro.Controls;
using MoneyChest.Data.Context;
using MoneyChest.Model.Enums;
using MoneyChest.Model.Model;
using MoneyChest.Services;
using MoneyChest.Services.Services;
using MoneyChest.Shared;
using MoneyChest.Shared.MultiLang;
using MoneyChest.Shared.Settings;
using MoneyChest.Utils;
using MoneyChest.View.Main;
using MoneyChest.ViewModel.Commands;
using MoneyChest.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MoneyChest
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : MetroWindow
    {
        #region Private fields
        
        private IUserService _userService;
        private LoginWindowViewModel _viewModel;

        #endregion

        #region Initialization

        public LoginWindow()
        {
            InitializeComponent();

            // init service
            _userService = ServiceManager.ConfigureService<UserService>();
            comboLanguages.ItemsSource = MultiLangEnumHelper.ToCollection(typeof(Language));
            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            // init model
            _viewModel = new LoginWindowViewModel()
            {
                ChangeViewCommand = new Command(() =>
                {
                    _viewModel.FlipViewIndex = _viewModel.FlipViewIndex == 0 ? 1 : 0;

                    txtPassword1.Password = null;
                    txtPassword2.Password = null;
                    txtConfirmPassword.Password = null;
                }),
                LoginCommand = new Command(() =>
                {
                    var user = _viewModel.FlipViewIndex == 0 ? Login() : Register();
                    if (user == null) return;

                    // save global variables
                    GlobalVariables.UserId = user.Id;

                    // save settings
                    AppSettings.Instance.LastLogin = user.Name;
                    AppSettings.Instance.LastLanguage = user.Language;
                    AppSettings.Instance.Save();

                    // update user last usage
                    user.LastUsageDate = DateTime.Today;
                    _userService.Update(user);

                    // show main window
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                },
                () => !string.IsNullOrEmpty(_viewModel.Name) && !string.IsNullOrEmpty(_viewModel.Password)
                    && (_viewModel.FlipViewIndex == 0 || _viewModel.Password == _viewModel.ConfirmPassword)),

                CancelCommand = new Command(() => this.Close())
            };

            // add command validation on property changed
            _viewModel.PropertyChanged += (sender, e) => _viewModel.LoginCommand.ValidateCanExecute();
            // set datacontext 
            this.DataContext = _viewModel;
        }

        #endregion

        #region Event handlers

        private void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // prepare controls data
            _viewModel.Name = AppSettings.Instance.LastLogin;
            if(AppSettings.Instance.LastLanguage.HasValue)
            {
                _viewModel.Language = AppSettings.Instance.LastLanguage.Value;
                MultiLangResourceManager.Instance.SetLanguage(AppSettings.Instance.LastLanguage.Value);
            }
            else
            {
                _viewModel.Language = MultiLangUtils.GetLanguage(CultureInfo.CurrentUICulture.Name);
                MultiLangResourceManager.Instance.SetLanguage(_viewModel.Language);
            }
            txtPassword1.Focus();
        }
        
        private void Password_Changed(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            _viewModel.Password = passwordBox.Password;
        }

        private void ConfirmPassword_Changed(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            _viewModel.ConfirmPassword = passwordBox.Password;
        }

        private void comboLanguages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(comboLanguages.SelectedValue != null)
                MultiLangResourceManager.Instance.SetLanguage((Language)comboLanguages.SelectedValue);
        }

        #endregion

        #region Private methods

        private UserModel Login()
        {
            var user = _userService.Get(_viewModel.Name, _viewModel.Password);
            if (user == null)
            {
                MessageBox.Show(MultiLangResourceManager.Instance[MultiLangResourceName.LoginFailedMessage], MultiLangResourceManager.Instance[MultiLangResourceName.LoginFailed], MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            MultiLangResourceManager.Instance.SetLanguage(user.Language);

            return user;
        }

        private UserModel Register()
        {
            // TODO: check ConfirmPassword

            var user = _userService.Get(_viewModel.Name);
            if (user != null)
            {
                MessageBox.Show(MultiLangResourceManager.Instance[MultiLangResourceName.RegistrationFailedMessage], MultiLangResourceManager.Instance[MultiLangResourceName.RegistrationFailed], MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            return _userService.Add(new Model.Model.UserModel()
            {
                Name = _viewModel.Name,
                Password = _viewModel.Password,
                Language = _viewModel.Language
            });
        }

        #endregion
    }
}
