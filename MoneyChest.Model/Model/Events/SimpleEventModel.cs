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
        #region Initialization

        public SimpleEventModel() : base()
        {
            RecordType = RecordType.Expense;
            EventType = EventType.Simple;
        }

        #endregion

        #region Entity properties

        public RecordType RecordType { get; set; }

        public int CurrencyId { get; set; }
        public int? CategoryId { get; set; }
        public int StorageId { get; set; }
        
        public CurrencyReference Currency { get; set; }
        public CategoryReference Category { get; set; }
        public StorageReference Storage { get; set; }

        public override decimal Value { get; set; }
        public override decimal CurrencyExchangeRate { get; set; }
        public override decimal Commission
        {
            get => base.Commission;
            set => base.Commission = value;
        }
        public override CommissionType CommissionType { get; set; }

        #endregion

        #region Transaction characteristics

        public override TransactionType TransactionType => 
            RecordType == RecordType.Income ? TransactionType.Income : TransactionType.Expense;
        public override string TransactionValueDetailed => ResultValueSignCurrency;
        public override string TransactionStorageDetailed => Storage?.Name;
        public override int[] TransactionStorageIds => new[] { StorageId };
        public override CategoryReference TransactionCategory => Category;
        public override StorageReference TransactionStorage => Storage;
        public override int TransactionCurrencyId => Storage?.CurrencyId ?? CurrencyId;
        public override decimal TransactionAmount => Storage?.CurrencyId != CurrencyId ? ResultValueSignExchangeRate : ResultValueSign;

        #endregion

        #region Additional properties

        [DependsOn(nameof(Value), nameof(CurrencyExchangeRate), nameof(Commission), nameof(CommissionType))]
        public decimal ResultValueSign => RecordType == RecordType.Expense ? -ResultValue : ResultValue;
        // TODO: CurrencyExchangeRate
        public string ResultValueSignCurrency => Currency?.FormatValue(ResultValueSign) ?? ResultValueSign.ToString("0.##");

        public decimal ResultValueExchangeRate => Storage?.CurrencyId != CurrencyId ? ResultValue * CurrencyExchangeRate : ResultValue;
        public decimal ResultValueSignExchangeRate => Storage?.CurrencyId != CurrencyId ? ResultValueSign * CurrencyExchangeRate : ResultValueSign;

        #endregion
    }
}
