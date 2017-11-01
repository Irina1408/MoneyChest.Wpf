using MahApps.Metro.Controls;
using MoneyChest.Data.Context;
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

        private IUserService userService;

        #endregion

        #region Initialization

        public LoginWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Event handlers

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // TODO: replace ApplicationDbContext initialization
            var context = ApplicationDbContext.Create();
            userService = new UserService(context);
            txtName.Text = UserSettings.Instance.LastLogin;
            txtPassword.Focus();

            // TODO: to be removed
            btnOk.IsEnabled = true;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: EntityFramework reference
            //var user = userService.Get(txtName.Text, txtPassword.Password);
            //if(user == null)
            //{
            //    // TODO: show login failed
            //    user = userService.Add(new Model.Model.UserModel()
            //    {
            //        Name = txtPassword.Name,
            //        Password = txtPassword.Password
            //    }, Data.Enums.Language.English);
            //}

            //GlobalVariables.UserId = user.Id;
            GlobalVariables.Language = Model.Enums.Language.English;

            // save settings
            UserSettings.Instance.LastLogin = txtName.Text;
            UserSettings.Instance.Save();

            // show main window
            var mainWindow = new MainWindow();
            mainWindow.Show();
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
