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

namespace MoneyChest.View.Components
{
    /// <summary>
    /// Interaction logic for CommissionControl.xaml
    /// </summary>
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public partial class CommissionControl : UserControl
    {
        public CommissionControl()
        {
            InitializeComponent();
        }

        #region ShowTakeCommissionFromReceiver Property

        public bool ShowTakeCommissionFromReceiver
        {
            get => (bool)this.GetValue(ShowTakeCommissionFromReceiverProperty);
            set => this.SetValue(ShowTakeCommissionFromReceiverProperty, value);
        }

        public static readonly DependencyProperty ShowTakeCommissionFromReceiverProperty = DependencyProperty.Register(
            nameof(ShowTakeCommissionFromReceiver), typeof(bool), typeof(CommissionControl), new PropertyMetadata(false));

        #endregion

        #region CurrencySymbol Property

        public string CurrencySymbol
        {
            get => (string)this.GetValue(CurrencySymbolProperty);
            set => this.SetValue(CurrencySymbolProperty, value);
        }

        public static readonly DependencyProperty CurrencySymbolProperty = DependencyProperty.Register(
            nameof(CurrencySymbol), typeof(string), typeof(CommissionControl));

        #endregion
    }
}
