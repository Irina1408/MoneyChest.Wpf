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
    /// Interaction logic for DescriptionInput.xaml
    /// </summary>
    public partial class DescriptionInput : UserControl
    {
        public DescriptionInput()
        {
            InitializeComponent();
        }

        #region Description Property

        public string Description
        {
            get => (string)this.GetValue(DescriptionProperty);
            set => this.SetValue(DescriptionProperty, value);
        }

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            nameof(Description), typeof(string), typeof(DescriptionInput));

        #endregion
    }
}
