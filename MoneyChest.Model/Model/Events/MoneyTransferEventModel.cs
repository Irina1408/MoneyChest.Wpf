using MoneyChest.Model.Enums;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class MoneyTransferEventModel : EventModel
    {
        #region Initialization

        public MoneyTransferEventModel() : base()
        {
            CurrencyExchangeRate = 1;
            TakeCommissionFromReceiver = false;
            EventType = EventType.MoneyTransfer;
            CommissionType = CommissionType.Currency;
        }

        #endregion

        #region Entity properties

        public bool TakeCommissionFromReceiver { get; set; }
        
        public int StorageFromId { get; set; }
        public int StorageToId { get; set; }
        public int? CategoryId { get; set; }


        public StorageReference StorageFrom { get; set; }
        public StorageReference StorageTo { get; set; }
        public CurrencyReference StorageFromCurrency { get; set; }
        public CurrencyReference StorageToCurrency { get; set; }
        public CategoryReference Category { get; set; }
        
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

        public override TransactionType TransactionType => TransactionType.MoneyTransfer;
        public override string TransactionValueDetailed => ValueTransfering;
        public override bool IsExpense => Commission > 0;
        public override bool IsIncome => Commission < 0;
        public override string TransactionStorageDetailed => $"{StorageFrom?.Name} -> {StorageTo?.Name}";
        public override int[] TransactionStorageIds => new[] { StorageFromId, StorageToId };
        public override CategoryReference TransactionCategory => Category;
        public override int TransactionCurrencyId => TakeCommissionFromReceiver ? StorageTo.CurrencyId : StorageFrom.CurrencyId;
        public override decimal TransactionAmount => TakeCommissionFromReceiver ? -StorageToCommissionValue : -StorageFromCommissionValue;

        #endregion

        #region Additional properties

        public bool IsDifferentCurrenciesSelected =>
            StorageFromCurrency != null && StorageToCurrency != null && StorageFromCurrency.Id != StorageToCurrency.Id;

        [DependsOn(nameof(Value), nameof(CurrencyExchangeRate), nameof(Commission), nameof(CommissionType))]
        private decimal StorageFromCommissionValue => CommissionValue;

        [DependsOn(nameof(Value), nameof(CurrencyExchangeRate), nameof(Commission), nameof(CommissionType))]
        private decimal StorageToCommissionValue => IsDifferentCurrenciesSelected ? CommissionValue * CurrencyExchangeRate : CommissionValue;
        
        public decimal StorageFromCommission => TakeCommissionFromReceiver ? 0 : StorageFromCommissionValue;
        public decimal StorageToCommission => TakeCommissionFromReceiver ? StorageToCommissionValue : 0;

        [DependsOn(nameof(Value))]
        public decimal StorageFromValue
        {
            get => Value + StorageFromCommission;
            set => Value = value - StorageFromCommission;
        }

        [DependsOn(nameof(Value), nameof(CurrencyExchangeRate), nameof(Commission), nameof(CommissionType))]
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
