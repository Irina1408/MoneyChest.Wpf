using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MoneyChest.Model.Constants;

namespace MoneyChest.Model.Model
{
    public class RecordTemplateModel : TransactionTemplateBase, IHasId, IHasUserId, IHasDescription, IHasRemark
    {
        #region Initialization

        public RecordTemplateModel()
        {
            RecordType = RecordType.Expense;
            CurrencyExchangeRate = 1;
        }

        #endregion

        #region Entity properties

        public int Id { get; set; }
        public RecordType RecordType { get; set; }
        public decimal Value { get; set; }
        public decimal CurrencyExchangeRate { get; set; }
        public decimal Commission { get; set; }
        public CommissionType CommissionType { get; set; }

        [StringLength(MaxSize.DescriptionLength)]
        public string Description { get; set; }

        [StringLength(MaxSize.RemarkLength)]
        public string Remark { get; set; }

        public int? CategoryId { get; set; }
        public int CurrencyId { get; set; }
        public int StorageId { get; set; }
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

        public override bool IsExpense => RecordType == RecordType.Expense;
        public override bool IsIncome => RecordType == RecordType.Income;
        public override TransactionType TransactionType =>
            RecordType == RecordType.Income ? TransactionType.Income : TransactionType.Expense;
        public string TransactionValueDetailed => ResultValueSignCurrency;
        public string TransactionStorageDetailed => Storage?.Name;
        public int[] TransactionStorageIds => new[] { StorageId };
        public CategoryReference TransactionCategory => Category;
        public StorageReference TransactionStorage => Storage;
        public int TransactionCurrencyId => Storage?.CurrencyId ?? CurrencyId;
        public decimal TransactionAmount => Storage?.CurrencyId != CurrencyId ? ResultValueSignExchangeRate : ResultValueSign;

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
