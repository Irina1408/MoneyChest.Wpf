using MoneyChest.Model.Base;
using MoneyChest.Model.Constants;
using MoneyChest.Model.Enums;
using MoneyChest.Model.Extensions;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class MoneyTransferTemplateModel : TransactionTemplateBase, IHasId, IHasDescription, IHasCategory, IHasExchangeRate, IHasRemark
    {        
        #region Initialization

        public MoneyTransferTemplateModel()
        {
            CurrencyExchangeRate = 1;
            TakeCommissionFromReceiver = false;
        }

        #endregion

        #region Main entity properties

        public int Id { get; set; }
        
        public decimal Value { get; set; }  // always in StorageFrom currency
        public decimal CurrencyExchangeRate { get; set; }
        public bool SwappedCurrenciesRate { get; set; }
        public decimal Commission { get; set; } // always in StorageFrom currency if CommissionType == Currency
        public CommissionType CommissionType { get; set; }
        public bool TakeCommissionFromReceiver { get; set; }

        [StringLength(MaxSize.DescriptionLength)]
        public string Description { get; set; }

        [StringLength(MaxSize.RemarkLength)]
        public string Remark { get; set; }


        public int StorageFromId { get; set; }
        public int StorageToId { get; set; }
        public int? CategoryId { get; set; }

        #endregion

        #region References

        public StorageReference StorageFrom { get; set; }
        public StorageReference StorageTo { get; set; }
        public CurrencyReference StorageFromCurrency { get; set; }
        public CurrencyReference StorageToCurrency { get; set; }
        public CategoryReference Category { get; set; }

        #endregion

        #region Transaction overrides

        public override TransactionType TransactionType => TransactionType.MoneyTransfer;
        public string TransactionValueDetailed => ValueTransfering;
        public bool IsPlanned => false;
        public override bool IsExpense => Commission > 0;
        public override bool IsIncome => Commission < 0;
        public string TransactionStorageDetailed => $"{StorageFrom?.Name} -> {StorageTo?.Name}";
        public int[] TransactionStorageIds => new[] { StorageFromId, StorageToId };
        public CategoryReference TransactionCategory => Category;
        public StorageReference TransactionStorage => TakeCommissionFromReceiver ? StorageTo : StorageFrom;
        public int TransactionCurrencyId => TakeCommissionFromReceiver ? StorageTo.CurrencyId : StorageFrom.CurrencyId;
        public decimal TransactionAmount => TakeCommissionFromReceiver ? -StorageToCommissionValue : -StorageFromCommissionValue;
        
        #endregion

        #region Additional properties

        public bool IsDifferentCurrenciesSelected =>
            StorageFromCurrency != null && StorageToCurrency != null && StorageFromCurrency.Id != StorageToCurrency.Id;

        private decimal StorageFromCommissionValue => CommissionType == CommissionType.Currency ? Commission : Commission / 100 * Value;

        [DependsOn(nameof(CurrencyExchangeRate), nameof(SwappedCurrenciesRate))]
        private decimal StorageToCommissionValue => CommissionType == CommissionType.Currency
            ? (IsDifferentCurrenciesSelected ? Commission * this.ActualRate() : Commission)
            : Commission / 100 * (IsDifferentCurrenciesSelected ? Value * this.ActualRate() : Value);

        public decimal StorageFromCommission => TakeCommissionFromReceiver ? 0 : StorageFromCommissionValue;
        public decimal StorageToCommission => TakeCommissionFromReceiver ? StorageToCommissionValue : 0;

        public decimal StorageFromValue
        {
            get => Value + StorageFromCommission;
            set => Value = value - StorageFromCommission;
        }

        [DependsOn(nameof(CurrencyExchangeRate), nameof(SwappedCurrenciesRate))]
        public decimal StorageToValue
        {
            get => IsDifferentCurrenciesSelected ? Value * this.ActualRate() - StorageToCommission : Value - StorageToCommission;
            set
            {
                // take into account currency exchange rate and commission
                if (IsDifferentCurrenciesSelected && this.ActualRate() != 0)
                    Value = value / this.ActualRate() + StorageToCommission;
                else
                    Value = value + StorageToCommission;
            }
        }

        public string ValueTransfering => $"{StorageFromCurrency.FormatValue(StorageFromValue)} -> {StorageToCurrency.FormatValue(StorageToValue)}";

        #endregion
    }
}
