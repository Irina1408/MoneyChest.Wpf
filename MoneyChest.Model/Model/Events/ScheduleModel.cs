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

        public ScheduleModel() : this(false)
        { }

        public ScheduleModel(bool populateDefaults)
        {
            ScheduleType = ScheduleType.Once;
            Period = 1;
            DayOfMonth = 1;

            Months = new ObservableCollection<Month>();
            DaysOfWeek = new ObservableCollection<DayOfWeek>();

            if(populateDefaults)
            {
                foreach (Month month in Enum.GetValues(typeof(Month)))
                    Months.Add(month);

                foreach (DayOfWeek dayOfWeek in Enum.GetValues(typeof(DayOfWeek)))
                    DaysOfWeek.Add(dayOfWeek);
            }
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
