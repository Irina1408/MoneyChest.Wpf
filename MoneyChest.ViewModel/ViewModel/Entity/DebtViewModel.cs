using MoneyChest.Model.Enums;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MoneyChest.ViewModel.ViewModel
{
    public class DebtViewModel : DebtModel
    {
        public DebtViewModel() : base()
        {

        }

        public DebtViewModel(DebtModel debt) : this()
        {
            Id = debt.Id;
            Description = debt.Description;
            DebtType = debt.DebtType;

            CurrencyExchangeRate = debt.CurrencyExchangeRate;
            Value = debt.Value;
            InitialFee = debt.InitialFee;
            PaidValue = debt.PaidValue;
            TakeInitialFeeFromStorage = debt.TakeInitialFeeFromStorage;

            PaymentType = debt.PaymentType;
            FixedAmount = debt.FixedAmount;
            InterestRate = debt.InterestRate;
            MonthCount = debt.MonthCount;

            TakingDate = debt.TakingDate;
            DueDate = debt.DueDate;
            RepayingDate = debt.RepayingDate;

            IsRepaid = debt.IsRepaid;
            Remark = debt.Remark;

            CurrencyId = debt.CurrencyId;
            CategoryId = debt?.CategoryId;
            StorageId = debt?.StorageId;
            UserId = debt.UserId;

            Currency = debt.Currency;
            Category = debt.Category;
            Storage = debt.Storage;
            Penalties = debt.Penalties;
        }

        #region Details view properties

        public bool PaymentTypeIsRate
        {
            get => PaymentType != DebtPaymentType.FixedAmount;
            set
            {
                var newValue = value ? DebtPaymentType.FixedRate : DebtPaymentType.FixedAmount;
                if (newValue != PaymentType) PaymentType = newValue;
            }
        }

        public bool PaymentTypeIsAnnualRate
        {
            get => PaymentType == DebtPaymentType.DifferentialPayment || PaymentType == DebtPaymentType.AnnuityPayment;
            set
            {
                var newValue = value ? DebtPaymentType.DifferentialPayment : DebtPaymentType.FixedRate;
                if (newValue != PaymentType) PaymentType = newValue;
            }
        }

        public bool IsDifferentCurrenciesSelected => Currency != null && StorageCurrency != null && CurrencyId != StorageCurrency.Id;
        public string ExchangeRateExample => Currency != null && StorageCurrency != null
            ? $"{Currency.FormatValue(1)} = {StorageCurrency.FormatValue(CurrencyExchangeRate)}"
            : null;

        #endregion

        [PropertyChanged.DoNotNotify]
        public ICommand AddPenaltyCommand { get; set; }
    }
}
