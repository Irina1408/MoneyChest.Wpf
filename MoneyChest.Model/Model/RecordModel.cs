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
    public class RecordModel : TransactionBase, IHasId, IHasUserId, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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

        //[StringLength(1000)]
        //public string Description { get; set; }

        //[StringLength(4000)]
        //public string Remark { get; set; }
        
        public int? CategoryId { get; set; }
        public int CurrencyId { get; set; }
        public int? StorageId { get; set; }
        public int? DebtId { get; set; }
        public int UserId { get; set; }

        #endregion

        #region Reference properties

        public CategoryReference Category { get; set; }
        public CurrencyReference Currency { get; set; }
        public StorageReference Storage { get; set; }
        public DebtReference Debt { get; set; }

        #endregion

        #region Transaction overrides

        public override TransactionType TransactionType =>
            RecordType == RecordType.Income ? TransactionType.Income : TransactionType.Expense;
        public override string TransactionValueDetailed => ResultValueSignCurrency;
        public override DateTime TransactionDate => Date;
        public override bool IsPlanned => false;
        public override string TransactionStorageDetailed => Storage?.Name;
        public override int[] TransactionStorageIds => StorageId.HasValue ? new[] { StorageId.Value } : new[] { -1 };
        public override CategoryReference TransactionCategory => Category;
        public override int TransactionCurrencyId => Storage?.CurrencyId ?? CurrencyId;
        public override decimal TransactionAmount => Storage?.CurrencyId != CurrencyId ? ResultValueSignExchangeRate : ResultValueSign;

        #endregion

        #region Additional properties

        public decimal CommissionValue => CommissionType == CommissionType.Currency ? Commission : Commission / 100 * Value;
        public decimal ResultValue => Value + (IsExpense ? CommissionValue : -CommissionValue);
        public decimal ResultValueSign => IsExpense ? -ResultValue : ResultValue;
        // TODO: CurrencyExchangeRate
        public string ResultValueSignCurrency => Currency?.FormatValue(ResultValueSign) ?? ResultValueSign.ToString("0.##");
        public bool IsTypeSelectionAllowed => !DebtId.HasValue;
        public int? CurrencyIdForRate => Debt != null && Debt.CurrencyId != CurrencyId ? Debt.CurrencyId : Storage?.CurrencyId;

        // TODO: check service. Removed value from storage and from debt. Case when currency exchange rate is for debt currency
        public decimal ResultValueExchangeRate => CurrencyIdForRate.HasValue && CurrencyIdForRate.Value != CurrencyId ? ResultValue * CurrencyExchangeRate : ResultValue;
        public decimal ResultValueSignExchangeRate => CurrencyIdForRate.HasValue && CurrencyIdForRate.Value != CurrencyId ? ResultValueSign * CurrencyExchangeRate : ResultValueSign;
        public bool IsIncomeRecordType
        {
            get => RecordType == RecordType.Income;
            set => RecordType = value ? RecordType.Income : RecordType.Expense;
        }

        #endregion
    }
}
