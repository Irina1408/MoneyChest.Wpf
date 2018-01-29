using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Model.Enums;
using PropertyChanged;

namespace MoneyChest.Model.Model
{
    public class SimpleEventModel : EventModel
    {
        public SimpleEventModel() : base()
        {
            TransactionType = TransactionType.Expense;
            EventType = EventType.Simple;
        }

        public TransactionType TransactionType { get; set; }

        public int CurrencyId { get; set; }
        public int? CategoryId { get; set; }
        public int StorageId { get; set; }

        
        public CurrencyReference Currency { get; set; }
        public CategoryReference Category { get; set; }
        public StorageReference Storage { get; set; }
        public CurrencyReference StorageCurrency { get; set; }

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
        public decimal ResultValueSign => TransactionType == TransactionType.Expense ? -ResultValue : ResultValue;
        public string ResultValueSignCurrency => Currency?.FormatValue(ResultValueSign) ?? ResultValueSign.ToString("0.##");

        public bool IsDifferentCurrenciesSelected => StorageCurrency?.Id != Currency?.Id;
        public string ExchangeRateExample => Currency != null && StorageCurrency != null
            ? $"{Currency.FormatValue(1)} = {StorageCurrency.FormatValue(CurrencyExchangeRate)}"
            : null;

        #endregion
    }
}
