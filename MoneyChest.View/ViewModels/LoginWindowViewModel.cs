using MoneyChest.Model.Enums;
using MoneyChest.View.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MoneyChest.View.ViewModels
{
    public class LoginWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public string Name { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public Language Language { get; set; }

        public int FlipViewIndex { get; set; }
        public string LoginButtonLabel => FlipViewIndex == 0 ? "Log In" : "Register";    // TODO: language
        public Visibility ExclamationConfirmPasswordVisibility => 
            string.IsNullOrEmpty(Password) || Password == ConfirmPassword ? Visibility.Hidden : Visibility.Visible;

        public IMCCommand ChangeViewCommand { get; set; }
        public IMCCommand LoginCommand { get; set; }
        public IMCCommand CancelCommand { get; set; }
    }
}
