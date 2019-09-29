using MoneyChest.Model.Base;
using MoneyChest.Model.Constants;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class DebtModel : IHasId, IHasUserId, IHasDescription, IHasCategory, IHasRemark, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<DebtPenaltyModel> penalties;

        #region Initialization

        public DebtModel()
        {
            TakingDate = DateTime.Today;
            IsRepaid = false;
            PaymentType = DebtPaymentType.FixedAmount;
            CurrencyExchangeRate = 1;
            MonthCount = 12;
            DebtType = DebtType.TakeBorrow;
            TakeInitialFeeFromStorage = true;
            Penalties = new ObservableCollection<DebtPenaltyModel>();
        }

        #endregion

        #region Main properties

        public int Id { get; set; }

        [StringLength(MaxSize.DescriptionLength)]
        public string Description { get; set; }
        public DebtType DebtType { get; set; }

        public decimal CurrencyExchangeRate { get; set; }   // if exists StorageId -> for add/remove money to/from storage
        public decimal Value { get; set; }                  // initial value that will be added/removed to/from storage
        public decimal InitialFee { get; set; }  // initial paid value
        public decimal PaidValue { get; set; }  // paid value by user records in Money Chest
        public bool TakeInitialFeeFromStorage { get; set; } // TODO: to be removed

        // payment conditions
        public DebtPaymentType PaymentType { get; set; }
        public decimal FixedAmount { get; set; }
        public decimal InterestRate { get; set; }     // Percentage
        public int MonthCount { get; set; }
        
        public DateTime TakingDate { get; set; }
        public DateTime? DueDate { get; set; }  // user sets date when debt should be repaid. just for user notifications
        public DateTime? RepayingDate { get; set; } // date of setting IsRepaid=true

        public bool IsRepaid { get; set; }

        [StringLength(MaxSize.RemarkLength)]
        public string Remark { get; set; }

        #endregion

        #region Foreign key properties

        public int CurrencyId { get; set; }
        public int? CategoryId { get; set; }
        public int? StorageId { get; set; }
        public int UserId { get; set; }

        #endregion

        #region References

        public CurrencyReference Currency { get; set; }
        public CategoryReference Category { get; set; }
        public StorageReference Storage { get; set; }
        public ObservableCollection<DebtPenaltyModel> Penalties
        {
            get => penalties;
            set
            {
                penalties = value;
                // add nofitication for every existing penalty on value is changed
                foreach(var penalty in penalties)
                    penalty.PropertyChanged += PenaltyPropertyChanged;

                penalties.CollectionChanged += (sender, e) =>
                {
                    // add nofitication for every penalty on value is changed
                    if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                        foreach (DebtPenaltyModel newItem in e.NewItems)
                            newItem.PropertyChanged += PenaltyPropertyChanged;

                    // remove nofitication for every removed penalty
                    else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                    {
                        // refresh view
                        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValueToBePaidCurrency)));
                        // remove event
                        foreach (DebtPenaltyModel oldItem in e.OldItems)
                            oldItem.PropertyChanged -= PenaltyPropertyChanged;
                    }
                };
            }
        }

        private void PenaltyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // refresh view
            if (e.PropertyName == nameof(DebtPenaltyModel.Value))
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValueToBePaidCurrency)));
        }

        #endregion

        #region Additional properties

        public string DescriptionRemainsToPayCurrency => $"{Description} ({RemainsToPayCurrency})";
        public string ValueCurrency => Currency?.FormatValue(Value);
        public string RemainsToPayCurrency => Currency?.FormatValue(RemainsToPay);
        public string ValueToBePaidCurrency => Currency?.FormatValue(ValueToBePaid) ?? ValueToBePaid.ToString("0.##");

        public decimal Progress => ValueToBePaid != 0 ? RemainsToPay / ValueToBePaid : 0;
        public decimal RemainsToPay => ValueToBePaid - InitialFee - PaidValue;
        public decimal ValueToBePaid => ValueWithPercentage + Penalties.Sum(_ => _.Value);
        public decimal ValueWithPercentage
        {
            get
            {
                switch(PaymentType)
                {
                    case DebtPaymentType.FixedAmount:
                        return Value + FixedAmount;
                    case DebtPaymentType.FixedRate:
                        return Value + InterestRate / 100 * Value;
                    case DebtPaymentType.DifferentialPayment:
                        return Value + DifferentialPayment;
                    case DebtPaymentType.AnnuityPayment:
                        return AnnualPayment;
                    default:
                        return Value + FixedAmount;
                }
            }
        }

        private decimal DifferentialPayment
        {
            get
            {
                // check month count
                if (MonthCount == 0) return 0;
                // get average amount of credit value
                var averageAmount = Value / MonthCount;

                decimal result = 0;
                for (int i = 0; i < MonthCount; i++)
                    // = the balance you need to pay for the current month * AnnualRate / MonthCount
                    result += (Value - averageAmount * i) * (InterestRate / 100) / MonthCount;

                return result;
            }
        }
        private decimal AnnualPayment
        {
            get
            {
                // annual percentage
                var perc = InterestRate / 100 / 12;
                //=G2/(1-(1/(1+G2))^H2)
                var pow = (decimal)Math.Pow(decimal.ToDouble(1/(1 + perc)), MonthCount);
                return Value * (perc / (1 - pow)) * MonthCount;
                //return Value * (perc + (perc / (decimal)Math.Pow(decimal.ToDouble(1 + perc), MonthCount - 1))) * MonthCount;
            }
        }

        #endregion
    }
}
