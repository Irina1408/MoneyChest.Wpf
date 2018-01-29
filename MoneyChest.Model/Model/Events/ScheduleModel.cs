using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class ScheduleModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ScheduleModel()
        {
            ScheduleType = ScheduleType.Once;
            Period = 1;
        }

        public ScheduleType ScheduleType { get; set; }
        public int Period { get; set; }

        public int DayOfMonth { get; set; }
        public List<Month> Months { get; set; } = new List<Month>();

        public List<DayOfWeek> DaysOfWeek { get; set; } = new List<DayOfWeek>();

        public bool IsDateRange => ScheduleType != ScheduleType.Once;
    }
}
