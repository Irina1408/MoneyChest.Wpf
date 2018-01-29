using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Model.Enums;
using PropertyChanged;

namespace MoneyChest.Model.Model
{
    public class RepayDebtEventModel : EventModel
    {
        public RepayDebtEventModel() : base()
        {
            EventType = EventType.RepayDebt;
            IsValueInStorageCurrency = false;
        }

        public bool IsValueInStorageCurrency { get; set; }
        public int StorageId { get; set; }
        public int DebtId { get; set; }

        
        public StorageReference Storage { get; set; }
        public DebtReference Debt { get; set; }

        public CurrencyReference StorageCurrency { get; set; }
        public CurrencyReference DebtCurrency { get; set; }
        public CategoryReference DebtCategory { get; set; }
        public CurrencyReference Currency => IsValueInStorageCurrency ? StorageCurrency : DebtCurrency;
        public CurrencyReference CurrencyForRate => IsValueInStorageCurrency ? DebtCurrency : StorageCurrency;

        public override decimal Value { get; set; }
        public override decimal CurrencyExchangeRate { get; set; }
        public override decimal Commission
        {
            get => base.Commission;
            set => base.Commission = value;
        }
        public override CommissionType CommissionType { get; set; }

        #region Additional properties

        [DependsOn(nameof(Value), nameof(CurrencyExchangeRate), nameof(Commission), nameof(CommissionType))]
        public decimal ResultValueSign => Debt?.DebtType == DebtType.TakeBorrow ? -ResultValue : ResultValue;
        public string ResultValueSignCurrency => Currency?.FormatValue(ResultValueSign) ?? ResultValueSign.ToString("0.##");

        public bool IsDifferentCurrenciesSelected => StorageCurrency?.Id != DebtCurrency?.Id;
        public string ExchangeRateExample => Currency != null && CurrencyForRate != null
            ? $"{Currency.FormatValue(1)} = {CurrencyForRate.FormatValue(CurrencyExchangeRate)}"
            : null;

        #endregion
    }
}
