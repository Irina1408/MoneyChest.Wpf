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
    /// Interaction logic for NameInput.xaml
    /// </summary>
    public partial class NameInput : UserControl
    {
        public NameInput()
        {
            InitializeComponent();
        }

        #region Name Property

        public string EntityName
        {
            get => (string)this.GetValue(EntityNameProperty);
            set => this.SetValue(EntityNameProperty, value);
        }

        public static readonly DependencyProperty EntityNameProperty = DependencyProperty.Register(
            nameof(EntityName), typeof(string), typeof(NameInput));

        #endregion
    }
}
