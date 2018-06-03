using MoneyChest.Model.Base;
using MoneyChest.Model.Constants;
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
    public abstract class EventModel : IHasId, IHasUserId, IHasDescription, IHasRemark, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Private fields

        private bool _commissionEnabled;
        private decimal _commission;
        private ScheduleModel _schedule;
        private ActualEventState _actualEventState;
        private bool _refreshActualEventState;
        private EventState _eventState;

        #endregion

        #region Initialization

        public EventModel()
        {
            DateFrom = DateTime.Today.AddDays(1);
            EventState = EventState.Active;
            AutoExecution = false;
            ConfirmBeforeExecute = false;
            _commissionEnabled = false;
            Schedule = new ScheduleModel(true);
            TakeExistingCurrencyExchangeRate = true;
            _refreshActualEventState = true;
            CurrencyExchangeRate = 1;
        }

        #endregion

        #region Main entity properties

        public int Id { get; set; }

        [StringLength(MaxSize.DescriptionLength)]
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

        public virtual EventState EventState
        {
            get => _eventState;
            set
            {
                _eventState = value;
                _refreshActualEventState = true;
            }
        }

        public virtual ActualEventState ActualEventState
        {
            get
            {
                if (!_refreshActualEventState) return _actualEventState;

                _actualEventState = EventState == EventState.Closed 
                    ? ActualEventState.Closed 
                    : (EventState == EventState.Paused ? ActualEventState.Paused : ActualEventState.Active);

                // check current result is planned state
                if (_actualEventState == ActualEventState.Paused && PausedToDate != null && PausedToDate <= DateTime.Today)
                    _actualEventState = ActualEventState.Active;

                // check active event on planned/finished state
                if (_actualEventState == ActualEventState.Active && DateFrom > DateTime.Today)
                    _actualEventState = ActualEventState.Planned;
                else if (_actualEventState == ActualEventState.Active && (DateUntil.HasValue && DateUntil.Value < DateTime.Today
                    || Schedule.ScheduleType == ScheduleType.Once && DateFrom < DateTime.Today))
                    _actualEventState = ActualEventState.Finished;

                _refreshActualEventState = false;
                return _actualEventState;
            }
        }

        public EventType EventType { get; set; }    // TODO: remove?
        
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


        [StringLength(MaxSize.RemarkLength)]
        public string Remark { get; set; }
        
        public int UserId { get; set; }

        #endregion

        #region Transaction characteristics

        public virtual bool IsExpense => TransactionType == TransactionType.Expense;
        public virtual bool IsIncome => TransactionType == TransactionType.Income;

        public abstract TransactionType TransactionType { get; }
        public abstract string TransactionValueDetailed { get; }
        public abstract string TransactionStorageDetailed { get; }
        public abstract int[] TransactionStorageIds { get; }
        public abstract CategoryReference TransactionCategory { get; }
        public abstract StorageReference TransactionStorage { get; }
        public abstract int TransactionCurrencyId { get; }
        public abstract decimal TransactionAmount { get; }

        #endregion

        #region Additional properties

        public virtual bool IsCurrencyExchangeRateRequired => CurrencyFromId != CurrencyToId;
        public abstract int CurrencyFromId { get; }
        public abstract int CurrencyToId { get; }

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
        public decimal ResultValue => Value + (IsExpense ? CommissionValue : -CommissionValue);
        //public abstract bool IsCurrencyExchangeRateRequired { get; }

        #endregion

        #region Methods

        public void NotifyScheduleChanged() => NotifyPropertyChanged(nameof(Schedule));
        protected void NotifyPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        #endregion
    }
}
