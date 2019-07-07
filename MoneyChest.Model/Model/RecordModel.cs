using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MoneyChest.Model.Model
{
    public class RecordModel : TransactionBase, IHasId, IHasUserId, IHasDescription, IHasRemark, INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        #region Initialization

        public RecordModel()
        {
            Date = DateTime.Now;
            RecordType = RecordType.Expense;
            CurrencyExchangeRate = 1;
        }

        #endregion

        #region Entity properties

        //public int Id { get; set; }
        public DateTime Date { get; set; }
        public RecordType RecordType { get; set; }
        public decimal Value { get; set; }
        public decimal CurrencyExchangeRate { get; set; }
        public decimal Commission { get; set; }
        public CommissionType CommissionType { get; set; }
        public bool IsAutoExecuted { get; set; }

        public int? CategoryId { get; set; }
        public int CurrencyId { get; set; }
        public int StorageId { get; set; }
        public int? DebtId { get; set; }
        public int? EventId { get; set; }
        public int UserId { get; set; }

        #endregion

        #region Reference properties

        public CategoryReference Category { get; set; }
        public CurrencyReference Currency { get; set; }
        public StorageReference Storage { get; set; }
        public DebtReference Debt { get; set; }

        #endregion

        #region Transaction overrides

        public override bool IsExpense => RecordType == RecordType.Expense;
        public override bool IsIncome => RecordType == RecordType.Income;
        public override TransactionType TransactionType =>
            RecordType == RecordType.Income ? TransactionType.Income : TransactionType.Expense;
        public override string TransactionValueDetailed => ResultValueSignCurrency;
        public override DateTime TransactionDate => Date;
        public override bool IsPlanned => false;
        public override string TransactionStorageDetailed => Storage?.Name;
        public override int[] TransactionStorageIds => new[] { StorageId };
        public override CategoryReference TransactionCategory => Category;
        public override StorageReference TransactionStorage => Storage;
        public override int TransactionCurrencyId => Storage?.CurrencyId ?? CurrencyId;
        public override decimal TransactionAmount => Storage?.CurrencyId != CurrencyId ? ResultValueSignExchangeRate : ResultValueSign;

        #endregion

        #region Additional properties

        public decimal CommissionValue => CommissionType == CommissionType.Currency ? Commission : Commission / 100 * Value;
        [PropertyChanged.DependsOn(nameof(RecordType))]
        public decimal ResultValue => Value + (IsExpense ? CommissionValue : -CommissionValue);
        public decimal ResultValueSign => IsExpense ? -ResultValue : ResultValue;
        // TODO: CurrencyExchangeRate
        public string ResultValueSignCurrency => Currency?.FormatValue(ResultValueSign, true) ?? ResultValueSign.ToString("0.##");
        public bool IsTypeSelectionAllowed => !DebtId.HasValue;
        public int? CurrencyIdForRate => Debt != null && Debt.CurrencyId != CurrencyId ? Debt.CurrencyId : Storage?.CurrencyId;
        public CurrencyReference CurrencyForRate => Debt != null && Debt.CurrencyId != CurrencyId ? Debt.Currency : Storage?.Currency;

        // TODO: check service. Removed value from storage and from debt. Case when currency exchange rate is for debt currency
        public decimal ResultValueExchangeRate => CurrencyIdForRate != CurrencyId ? ResultValue * CurrencyExchangeRate : ResultValue;
        public decimal ResultValueSignExchangeRate => CurrencyIdForRate != CurrencyId ? ResultValueSign * CurrencyExchangeRate : ResultValueSign;
        public string ResultValueSignExchangeRateCurrency => CurrencyIdForRate != CurrencyId ? CurrencyForRate?.FormatValue(ResultValueSignExchangeRate, true) ?? ResultValueSignExchangeRate.ToString("0.##") : null;
        public bool IsIncomeRecordType
        {
            get => RecordType == RecordType.Income;
            set => RecordType = value ? RecordType.Income : RecordType.Expense;
        }

        #endregion
    }
}
