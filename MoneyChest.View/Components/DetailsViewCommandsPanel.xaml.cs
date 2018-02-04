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
    /// Interaction logic for DetailsViewCommandsPanel.xaml
    /// </summary>
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public partial class DetailsViewCommandsPanel : UserControl
    {
        public DetailsViewCommandsPanel()
        {
            InitializeComponent();
        }

        #region SaveCommand Property

        public ICommand SaveCommand
        {
            get => (ICommand)this.GetValue(SaveCommandProperty);
            set => this.SetValue(SaveCommandProperty, value);
        }

        public static readonly DependencyProperty SaveCommandProperty = DependencyProperty.Register(
            nameof(SaveCommand), typeof(ICommand), typeof(DetailsViewCommandsPanel));

        #endregion

        #region CancelCommand Property

        public ICommand CancelCommand
        {
            get => (ICommand)this.GetValue(CancelCommandProperty);
            set => this.SetValue(CancelCommandProperty, value);
        }

        public static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register(
            nameof(CancelCommand), typeof(ICommand), typeof(DetailsViewCommandsPanel));

        #endregion
    }
}
