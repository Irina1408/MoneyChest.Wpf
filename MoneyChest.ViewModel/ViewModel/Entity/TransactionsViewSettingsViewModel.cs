using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;
using MoneyChest.Shared;
using MoneyChest.Shared.MultiLang;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.ViewModel.ViewModel
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class TransactionsViewSettingsViewModel : IHasUserId, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler OnPeriodChanged;

        private PeriodType _periodType;

        public TransactionsViewSettingsViewModel()
        {
            PeriodType = PeriodType.Month;
            PropertyChanged += (sender, e) =>
            {
                if(!IsDateRangeFilling && (e.PropertyName == nameof(DateFrom) || e.PropertyName == nameof(DateUntil)))
                    OnPeriodChanged?.Invoke(this, EventArgs.Empty);
            };
        }

        public int UserId { get; set; }
        public PeriodType PeriodType
        {
            get => _periodType;
            set
            {
                _periodType = value;
                AdaptDateRange(_periodType);
            }
        }
        public DateTime DateFrom { get; set; }
        public DateTime DateUntil { get; set; }
        public bool IsDateRangeFilling { get; set; }

        [PropertyChanged.DependsOn(nameof(PeriodType), nameof(DateFrom), nameof(DateUntil))]
        public string PeriodDetails => GetPeriodRangeDetails(PeriodType, DateFrom, DateUntil);

        // TODO: replace to some helper
        public void PrevDateRange()
        {
            IsDateRangeFilling = true;

            switch (PeriodType)
            {
                //case PeriodType.Day:
                //    DateFrom = DateFrom.AddDays(-1);
                //    DateUntil = DateUntil.AddDays(-1);
                //    break;

                //case PeriodType.Week:
                //    DateFrom = DateFrom.AddDays(-7);
                //    DateUntil = DateUntil.AddDays(-7);
                //    break;

                case PeriodType.Month:
                    DateFrom = DateFrom.AddMonths(-1);
                    DateUntil = DateUntil.AddMonths(-1);
                    break;

                case PeriodType.Quarter:
                    DateFrom = DateFrom.AddMonths(-3);
                    DateUntil = DateUntil.AddMonths(-3);
                    break;

                case PeriodType.Year:
                    DateFrom = DateFrom.AddYears(-1);
                    DateUntil = DateUntil.AddYears(-1);
                    break;

                default:
                    var diff = (DateUntil - DateFrom).Days + 1;
                    DateFrom = DateFrom.AddDays(-diff);
                    DateUntil = DateUntil.AddDays(-diff);
                    break;
            }
            IsDateRangeFilling = false;
            // notify period changed
            OnPeriodChanged?.Invoke(this, EventArgs.Empty);
        }

        // TODO: replace to some helper
        public void NextDateRange()
        {
            IsDateRangeFilling = true;

            switch (PeriodType)
            {
                //case PeriodType.Day:
                //    DateFrom = DateFrom.AddDays(1);
                //    DateUntil = DateUntil.AddDays(1);
                //    break;

                //case PeriodType.Week:
                //    DateFrom = DateFrom.AddDays(7);
                //    DateUntil = DateUntil.AddDays(7);
                //    break;

                case PeriodType.Month:
                    DateFrom = DateFrom.AddMonths(1);
                    DateUntil = DateUntil.AddMonths(1);
                    break;

                case PeriodType.Quarter:
                    DateFrom = DateFrom.AddMonths(3);
                    DateUntil = DateUntil.AddMonths(3);
                    break;

                case PeriodType.Year:
                    DateFrom = DateFrom.AddYears(1);
                    DateUntil = DateUntil.AddYears(1);
                    break;

                default:
                    var diff = (DateUntil - DateFrom).Days + 1;
                    DateFrom = DateFrom.AddDays(diff);
                    DateUntil = DateUntil.AddDays(diff);
                    break;
            }

            IsDateRangeFilling = false;
            // notify period changed
            OnPeriodChanged?.Invoke(this, EventArgs.Empty);
        }

        // TODO: replace to some helper
        private void AdaptDateRange(PeriodType periodType)
        {
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
            if(periodType != PeriodType.Custom) DateUntil = DateUntil.AddDays(1).AddMilliseconds(-1);
            IsDateRangeFilling = false;
            // notify period changed
            OnPeriodChanged?.Invoke(this, EventArgs.Empty);
        }

        // TODO: replace to some helper
        private string GetPeriodRangeDetails(PeriodType periodType, DateTime dateFrom, DateTime dateUntil)
        {
            switch (periodType)
            {
                case PeriodType.Day:
                    return dateFrom.ToShortDateString();

                case PeriodType.Month:
                    return $"{MultiLangResource.EnumItemDescription(typeof(Month), (Month)dateFrom.Month)} {dateFrom.Year}";

                case PeriodType.Quarter:
                    return $"Q{(dateFrom.Month - 1) / 3 + 1} {dateFrom.Year}";

                case PeriodType.Year:
                    return dateFrom.Year.ToString();

                default:
                    return $"{DateFrom.ToShortDateString()} - {DateUntil.ToShortDateString()}";
            }
        }
    }
}
