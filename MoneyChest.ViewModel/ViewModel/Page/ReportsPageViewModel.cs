using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.ViewModel.ViewModel
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class ReportsPageViewModel<TSpecial>
        where TSpecial: class, new()
    {
        public ReportSettingModel Settings { get; set; }
        public TSpecial Special { get; set; } = new TSpecial();
        public string Total { get; set; }
    }
}
