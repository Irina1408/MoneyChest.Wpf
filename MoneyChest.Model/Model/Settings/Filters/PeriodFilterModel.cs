using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class PeriodFilterModel : INotifyPropertyChanged
    {
        #region Public events

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler OnPeriodChanged;

        #endregion

        #region Private fields

        private DateTime _dateFrom;
        private DateTime _dateUntil;
        private PeriodType _periodType;
        private bool _isDateRangeFilling;
        private bool _isPeriodChanged;

        #endregion

        #region Initialization

        public PeriodFilterModel()
        {
            PeriodType = PeriodType.Month;
            _isPeriodChanged = false;
        }

        #endregion

        #region Public properties
        
        public PeriodType PeriodType
        {
            get => _periodType;
            set
            {
                _periodType = value;
                AdaptDateRange(_periodType);
            }
        }

        public DateTime DateFrom
        {
            get => _dateFrom;
            set
            {
                _dateFrom = value;

                // notify period changed if it isn't date range filling
                if (IsDateRangeFilling) _isPeriodChanged = true;
                else NotifyPeriodChanged();
            }
        }

        public DateTime DateUntil
        {
            get => _dateUntil;
            set
            {
                _dateUntil = value;

                // notify period changed if it isn't date range filling
                if (IsDateRangeFilling) _isPeriodChanged = true;
                else NotifyPeriodChanged();
            }
        }

        public bool IsDateRangeFilling
        {
            get => _isDateRangeFilling;
            set
            {
                _isDateRangeFilling = value;

                // if period was changed notify about it 
                if (!_isDateRangeFilling && _isPeriodChanged) NotifyPeriodChanged();
            }
        }

        [PropertyChanged.DependsOn(nameof(PeriodType), nameof(DateFrom), nameof(DateUntil))]
        public string PeriodDetails => GetPeriodRangeDetails(PeriodType, DateFrom, DateUntil);

        #endregion

        #region Private methods
        
        private void AdaptDateRange(PeriodType periodType)
        {
            // announce date range population is started
            IsDateRangeFilling = true;

            switch (periodType)
            {
                case PeriodType.Day:
                    DateFrom = DateTime.Today;
                    DateUntil = DateTime.Today;
                    break;

                case PeriodType.Week:
                    DateFrom = DateTime.Today.FirstDayOfWeek();
                    DateUntil = DateFrom.AddDays(6);
                    break;

                case PeriodType.Month:
                    DateFrom = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    DateUntil = DateFrom.AddMonths(1).AddDays(-1);
                    break;

                case PeriodType.Quarter:
                    DateFrom = new DateTime(DateTime.Today.Year, ((DateTime.Today.Month - 1) / 3 + 1) * 3 - 2, 1);
                    DateUntil = DateFrom.AddMonths(3).AddDays(-1);
                    break;

                case PeriodType.Year:
                    DateFrom = new DateTime(DateTime.Today.Year, 1, 1);
                    DateUntil = DateFrom.AddYears(1).AddDays(-1);
                    break;
            }

            // adapt date until
            if (periodType != PeriodType.Custom) DateUntil = DateUntil.AddDays(1).AddMilliseconds(-1);
            // announce date range population is finished
            IsDateRangeFilling = false;
        }
        
        private string GetPeriodRangeDetails(PeriodType periodType, DateTime dateFrom, DateTime dateUntil)
        {
            switch (periodType)
            {
                case PeriodType.Day:
                    return dateFrom.ToShortDateString();

                case PeriodType.Month:
                    return $"{dateFrom.ToString("MMMM")} {dateFrom.Year}";

                case PeriodType.Quarter:
                    return $"Q{(dateFrom.Month - 1) / 3 + 1} {dateFrom.Year}";

                case PeriodType.Year:
                    return dateFrom.Year.ToString();

                default:
                    return $"{DateFrom.ToShortDateString()} - {DateUntil.ToShortDateString()}";
            }
        }

        private void NotifyPeriodChanged()
        {
            // notify period was changed
            OnPeriodChanged?.Invoke(this, EventArgs.Empty);
            // set period changing is notified
            _isPeriodChanged = false;
        }

        #endregion
    }
}
