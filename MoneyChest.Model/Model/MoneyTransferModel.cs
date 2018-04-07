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
    public class MoneyTransferModel : ITransaction, IHasId, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Private fields

        private bool _comissionEnabled;

        #endregion

        #region Initialization

        public MoneyTransferModel()
        {
            CurrencyExchangeRate = 1;
            TakeCommissionFromReceiver = false;
            CommissionEnabled = false;
            Date = DateTime.Today;
        }

        #endregion

        #region Main entity properties

        public int Id { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }  // always in StorageFrom currency
        public decimal CurrencyExchangeRate { get; set; }
        public decimal Commission { get; set; } // always in StorageFrom currency if CommissionType == Currency
        public CommissionType CommissionType { get; set; }
        public bool TakeCommissionFromReceiver { get; set; }

        [StringLength(4000)]
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

        #region ITransaction implementation

        public DateTime TransactionDate => Date;
        public bool IsPlanned => false;
        public TransactionType TransactionType => TransactionType.MoneyTransfer;
        public string TransactionValueDetailed => ValueTransfering;
        public string TransactionStorageDetailed => $"{StorageFrom?.Name} -> {StorageTo?.Name}";
        public int[] TransactionStorageIds => new[] { StorageFromId, StorageToId };
        public CategoryReference TransactionCategory => Category;
        public bool IsExpense => Commission > 0;
        public bool IsIncome => Commission < 0;

        #endregion

        #region Additional properties

        public bool CommissionEnabled
        {
            get => _comissionEnabled;
            set
            {
                _comissionEnabled = value;
                if (!_comissionEnabled)
                    Commission = 0;
            }
        }

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
