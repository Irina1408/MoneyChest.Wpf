using MahApps.Metro.Controls;
using MoneyChest.Data.Context;
using MoneyChest.Services;
using MoneyChest.Services.Services;
using MoneyChest.Shared;
using MoneyChest.Shared.Settings;
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
        private bool _dispose;

        #endregion

        #region Initialization

        public LoginWindow()
        {
            InitializeComponent();

            // init service
            ServiceManager.Initialize();
            _userService = ServiceManager.ConfigureService<UserService>();
            _dispose = true;
        }

        #endregion

        #region Event handlers

        private void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // prepare controls data
            txtName.Text = AppSettings.Instance.LastLogin;
            txtPassword.Focus();
        }

        private void LoginWindow_Closing(object sender, CancelEventArgs e)
        {
            if(_dispose)
                ServiceManager.Dispose();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var user = _userService.Get(txtName.Text, txtPassword.Password);
            if (user == null)
            {
                // TODO: show login failed
                user = _userService.Add(new Model.Model.UserModel()
                {
                    Name = txtName.Text,
                    Password = txtPassword.Password
                }, Model.Enums.Language.English);
            }

            // save global variables
            GlobalVariables.UserId = user.Id;
            GlobalVariables.Language = Model.Enums.Language.English;

            // save settings
            AppSettings.Instance.LastLogin = txtName.Text;
            AppSettings.Instance.Save();

            // update user last usage
            user.LastUsageDate = DateTime.Today;
            _userService.Update(user);

            // show main window
            var mainWindow = new MainWindow();
            mainWindow.Show();
            _dispose = false;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SequrityData_Changed(object sender, RoutedEventArgs e)
        {
            btnOk.IsEnabled = !string.IsNullOrEmpty(txtName.Text) && !string.IsNullOrEmpty(txtPassword.Password);
        }

        #endregion
    }
}
