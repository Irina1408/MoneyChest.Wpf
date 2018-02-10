using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class ScheduleModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Month> monthes;
        private ObservableCollection<DayOfWeek> daysOfWeek;

        public ScheduleModel()
        {
            ScheduleType = ScheduleType.Once;
            Period = 1;

            Months = new ObservableCollection<Month>();
            DaysOfWeek = new ObservableCollection<DayOfWeek>();
        }

        public ScheduleType ScheduleType { get; set; }
        public int Period { get; set; }

        public int DayOfMonth { get; set; }
        public ObservableCollection<Month> Months
        {
            get => monthes;
            set
            {
                monthes = value;
                monthes.CollectionChanged += (sender, e) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Months)));
            }
        }

        public ObservableCollection<DayOfWeek> DaysOfWeek
        {
            get => daysOfWeek;
            set
            {
                daysOfWeek = value;
                daysOfWeek.CollectionChanged += (sender, e) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DaysOfWeek)));
            }
        }

        public bool IsDateRange => ScheduleType != ScheduleType.Once;
    }
}
