using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class CalendarSettingsModel : IHasUserId, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public CalendarSettingsModel()
        {
            ShowLimits = true;
            PeriodFilter = new PeriodFilterModel();
            DataFilter = new DataFilterModel();
        }

        public int UserId { get; set; }

        public bool ShowSettings { get; set; }
        public bool ShowLimits { get; set; }
        public bool ShowAllTransactionsPerDay { get; set; }
        public int MaxTransactionsCountPerDay { get; set; } = 3;
        public PeriodFilterModel PeriodFilter { get; set; }
        public DataFilterModel DataFilter { get; set; }

        public bool TransactionsCountPerDayAvailable => !ShowAllTransactionsPerDay;
    }
}
