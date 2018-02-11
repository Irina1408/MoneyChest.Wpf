using MahApps.Metro.IconPacks;
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

namespace MoneyChest.View.Pages
{
    /// <summary>
    /// Interaction logic for TransactionsPage.xaml
    /// </summary>
    public partial class TransactionsPage : PageBase
    {
        public TransactionsPage() : base()
        {
            InitializeComponent();
        }

        #region Overrides

        public override void Reload()
        {
            base.Reload();
        }

        #endregion
    }
}
