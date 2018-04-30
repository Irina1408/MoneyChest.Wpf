using MoneyChest.Shared.MultiLang;
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

namespace MoneyChest.View.Components.Input
{
    /// <summary>
    /// Interaction logic for AmountInput.xaml
    /// </summary>
    public partial class AmountInput : UserControl
    {
        public AmountInput()
        {
            InitializeComponent();
        }

        #region Amount Property

        public decimal Amount
        {
            get => (decimal)this.GetValue(AmountProperty);
            set => this.SetValue(AmountProperty, value);
        }

        public static readonly DependencyProperty AmountProperty = DependencyProperty.Register(
            nameof(Amount), typeof(decimal), typeof(AmountInput));

        #endregion

        #region Caption Property

        public string Caption
        {
            get => (string)this.GetValue(CaptionProperty);
            set => this.SetValue(CaptionProperty, value);
        }

        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register(
            nameof(Caption), typeof(string), typeof(AmountInput), 
            new PropertyMetadata(MultiLangResourceManager.Instance[MultiLangResourceName.Amount]));

        #endregion
    }
}
