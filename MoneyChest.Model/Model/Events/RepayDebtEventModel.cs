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
        #region Initialization

        public RepayDebtEventModel() : base()
        {
            EventType = EventType.RepayDebt;
            IsValueInStorageCurrency = false;
        }

        #endregion

        #region Entity properties

        public bool IsValueInStorageCurrency { get; set; }
        public int StorageId { get; set; }
        public int DebtId { get; set; }
                
        public StorageReference Storage { get; set; }
        public DebtReference Debt { get; set; }

        public CategoryReference DebtCategory { get; set; }
        public CurrencyReference StorageCurrency { get; set; }
        public CurrencyReference DebtCurrency { get; set; }
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

        #endregion

        #region Transaction characteristics

        public override TransactionType TransactionType =>
            Debt?.DebtType == DebtType.GiveBorrow ? TransactionType.Income : TransactionType.Expense;
        public override string TransactionValueDetailed => ResultValueSignCurrency;
        public override string TransactionStorageDetailed => Storage?.Name;
        public override int[] TransactionStorageIds => Storage != null ? new[] { Storage.Id } : new[] { -1 };
        public override CategoryReference TransactionCategory => DebtCategory;
        public override StorageReference TransactionStorage => Storage;
        public override int TransactionCurrencyId => Storage.CurrencyId;
        public override decimal TransactionAmount => IsValueInStorageCurrency ? ResultValueSign : ResultValueSign * CurrencyExchangeRate;

        #endregion

        #region Additional properties
        
        public override int CurrencyFromId => Currency?.Id ?? 0;
        public override int CurrencyToId => CurrencyForRate?.Id ?? 0;

        [DependsOn(nameof(Value), nameof(CurrencyExchangeRate), nameof(Commission), nameof(CommissionType))]
        public decimal ResultValueSign => Debt?.DebtType == DebtType.TakeBorrow ? -ResultValue : ResultValue;
        public string ResultValueSignCurrency => Currency?.FormatValue(ResultValueSign) ?? ResultValueSign.ToString("0.##");

        #endregion
    }
}
