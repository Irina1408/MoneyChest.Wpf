using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class EventModel : IHasId, IHasUserId, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Private fields

        private bool _commissionEnabled;
        private decimal _commission;
        private ScheduleModel _schedule;

        #endregion

        #region Initialization

        public EventModel()
        {
            DateFrom = DateTime.Today;
            EventState = EventState.Active;
            AutoExecution = false;
            ConfirmBeforeExecute = false;
            _commissionEnabled = false;
            Schedule = new ScheduleModel();
        }

        #endregion

        #region Main entity properties

        public int Id { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public virtual decimal Value { get; set; }

        public virtual decimal CurrencyExchangeRate { get; set; }

        public bool TakeExistingCurrencyExchangeRate { get; set; }

        public virtual decimal Commission
        {
            get => _commission;
            set
            {
                _commission = value;
                if (_commission > 0)
                    CommissionEnabled = true;
            }
        }

        public virtual CommissionType CommissionType { get; set; }

        public EventState EventState { get; set; }

        public EventType EventType { get; set; }
        
        public ScheduleModel Schedule
        {
            get => _schedule;
            set
            {
                _schedule = value;
                _schedule.PropertyChanged += (sender, e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Schedule)));
            }
        }
        
        public DateTime DateFrom { get; set; }
        
        public DateTime? DateUntil { get; set; }

        public DateTime? PausedToDate { get; set; }

        public bool AutoExecution { get; set; }
        
        public TimeSpan? AutoExecutionTime { get; set; }

        public bool ConfirmBeforeExecute { get; set; }


        [StringLength(4000)]
        public string Remark { get; set; }
        
        public int UserId { get; set; }

        #endregion
        
        #region Additional properties

        public bool CommissionEnabled
        {
            get => _commissionEnabled;
            set
            {
                _commissionEnabled = value;
                if (!_commissionEnabled)
                    Commission = 0;
            }
        }

        public decimal CommissionValue => CommissionType == CommissionType.Currency ? Commission : Commission / 100 * Value;
        public decimal ResultValue => Value - CommissionValue;

        #endregion

        public void NotifyScheduleChanged() => NotifyPropertyChanged(nameof(Schedule));
        protected void NotifyPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
}
