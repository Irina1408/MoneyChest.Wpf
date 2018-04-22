using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class MoneyTransferModel : TransactionBase, IHasId, INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged;
        
        #region Initialization

        public MoneyTransferModel()
        {
            CurrencyExchangeRate = 1;
            TakeCommissionFromReceiver = false;
            Date = DateTime.Today;
        }

        #endregion

        #region Main entity properties

        //public int Id { get; set; }
        
        public DateTime Date { get; set; }
        public decimal Value { get; set; }  // always in StorageFrom currency
        public decimal CurrencyExchangeRate { get; set; }
        public decimal Commission { get; set; } // always in StorageFrom currency if CommissionType == Currency
        public CommissionType CommissionType { get; set; }
        public bool TakeCommissionFromReceiver { get; set; }

        //[StringLength(1000)]
        //public override string Description { get; set; }

        //[StringLength(4000)]
        //public override string Remark { get; set; }


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
        public override string TransactionValueDetailed => ValueTransfering;
        public override DateTime TransactionDate => Date;
        public override bool IsPlanned => false;
        public override bool IsExpense => Commission > 0;
        public override bool IsIncome => Commission < 0;
        public override string TransactionStorageDetailed => $"{StorageFrom?.Name} -> {StorageTo?.Name}";
        public override int[] TransactionStorageIds => new[] { StorageFromId, StorageToId };
        public override CategoryReference TransactionCategory => Category;
        public override StorageReference TransactionStorage => TakeCommissionFromReceiver ? StorageTo : StorageFrom;
        public override int TransactionCurrencyId => TakeCommissionFromReceiver ? StorageTo.CurrencyId : StorageFrom.CurrencyId;
        public override decimal TransactionAmount => TakeCommissionFromReceiver ? -StorageToCommissionValue : -StorageFromCommissionValue;
        
        #endregion

        #region Additional properties

        public bool IsDifferentCurrenciesSelected =>
            StorageFromCurrency != null && StorageToCurrency != null && StorageFromCurrency.Id != StorageToCurrency.Id;

        private decimal StorageFromCommissionValue => CommissionType == CommissionType.Currency ? Commission : Commission / 100 * Value;
        private decimal StorageToCommissionValue => CommissionType == CommissionType.Currency
            ? (IsDifferentCurrenciesSelected ? Commission * CurrencyExchangeRate : Commission)
            : Commission / 100 * (IsDifferentCurrenciesSelected ? Value * CurrencyExchangeRate : Value);

        public decimal StorageFromCommission => TakeCommissionFromReceiver ? 0 : StorageFromCommissionValue;
        public decimal StorageToCommission => TakeCommissionFromReceiver ? StorageToCommissionValue : 0;

        public decimal StorageFromValue
        {
            get => Value + StorageFromCommission;
            set => Value = value - StorageFromCommission;
        }
        public decimal StorageToValue
        {
            get => IsDifferentCurrenciesSelected ? Value * CurrencyExchangeRate - StorageToCommission : Value - StorageToCommission;
            set
            {
                // take into account currency exchange rate and commission
                if (IsDifferentCurrenciesSelected && CurrencyExchangeRate != 0)
                    Value = value / CurrencyExchangeRate + StorageToCommission;
                else
                    Value = value + StorageToCommission;
            }
        }

        public string ValueTransfering => $"{StorageFromCurrency.FormatValue(StorageFromValue)} -> {StorageToCurrency.FormatValue(StorageToValue)}";

        #endregion
    }
}
