using MahApps.Metro.Controls;
using MoneyChest.Data.Context;
using MoneyChest.Model.Enums;
using MoneyChest.Model.Model;
using MoneyChest.Services;
using MoneyChest.Services.Services;
using MoneyChest.Shared;
using MoneyChest.Shared.Settings;
using MoneyChest.Utils;
using MoneyChest.View.Commands;
using MoneyChest.View.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private bool _dispose;

        #endregion

        #region Initialization

        public LoginWindow()
        {
            InitializeComponent();

            // init service
            ServiceManager.Initialize();
            _userService = ServiceManager.ConfigureService<UserService>();
            comboLanguages.ItemsSource = EnumHelper.ToListOfOriginalValueAndDescription(typeof(Language));
            InitializeViewModel();
            _dispose = true;
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
                    GlobalVariables.Language = user.Language;

                    // save settings
                    AppSettings.Instance.LastLogin = user.Name;
                    AppSettings.Instance.Save();

                    // update user last usage
                    user.LastUsageDate = DateTime.Today;
                    _userService.Update(user);

                    // show main window
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    _dispose = false;
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
            txtPassword1.Focus();
        }

        private void LoginWindow_Closing(object sender, CancelEventArgs e)
        {
            if(_dispose)
                ServiceManager.Dispose();
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

        #endregion

        #region Private methods

        private UserModel Login()
        {
            var user = _userService.Get(_viewModel.Name, _viewModel.Password);
            if (user == null)
            {
                MessageBox.Show("Login failed. Please verify your Name and Password.", "Login failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            return user;
        }

        private UserModel Register()
        {
            // TODO: check ConfirmPassword

            var user = _userService.Get(_viewModel.Name);
            if (user != null)
            {
                MessageBox.Show("User with the same Name already exists. Please enter other Name.", "Registration failed", MessageBoxButton.OK, MessageBoxImage.Error);
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
