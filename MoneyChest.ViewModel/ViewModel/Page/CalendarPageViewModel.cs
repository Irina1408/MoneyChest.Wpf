using MoneyChest.Calculation.Builders;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.ViewModel.ViewModel
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class CalendarPageViewModel
    {
        public CalendarSettingsModel Settings { get; set; }
        public List<CalendarDayData> Data { get; set; }
    }
}
