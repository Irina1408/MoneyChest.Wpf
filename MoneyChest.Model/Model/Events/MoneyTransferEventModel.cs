﻿using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;
using MoneyChest.Model.Extensions;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class MoneyTransferEventModel : EventModel, IHasCategory
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
        public override bool SwappedCurrenciesRate { get; set; }
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
        public override StorageReference TransactionStorage => TakeCommissionFromReceiver ? StorageTo : StorageFrom;
        public override int TransactionCurrencyId => TakeCommissionFromReceiver ? StorageTo.CurrencyId : StorageFrom.CurrencyId;
        public override decimal TransactionAmount => TakeCommissionFromReceiver ? -StorageToCommissionValue : -StorageFromCommissionValue;

        #endregion

        #region Additional properties
        
        public override int CurrencyFromId => StorageFromCurrency?.Id ?? 0;
        public override int CurrencyToId => StorageToCurrency?.Id ?? 0;

        [DependsOn(nameof(Value), nameof(CurrencyExchangeRate), nameof(Commission), nameof(CommissionType))]
        private decimal StorageFromCommissionValue => CommissionValue;

        [DependsOn(nameof(Value), nameof(CurrencyExchangeRate), nameof(SwappedCurrenciesRate), nameof(Commission), nameof(CommissionType))]
        private decimal StorageToCommissionValue => IsCurrencyExchangeRateRequired ? CommissionValue * this.ActualRate() : CommissionValue;
        
        public decimal StorageFromCommission => TakeCommissionFromReceiver ? 0 : StorageFromCommissionValue;
        public decimal StorageToCommission => TakeCommissionFromReceiver ? StorageToCommissionValue : 0;

        [DependsOn(nameof(Value))]
        public decimal StorageFromValue
        {
            get => Value + StorageFromCommission;
            set => Value = value - StorageFromCommission;
        }

        [DependsOn(nameof(Value), nameof(CurrencyExchangeRate), nameof(SwappedCurrenciesRate), nameof(Commission), nameof(CommissionType))]
        public decimal StorageToValue
        {
            get => IsCurrencyExchangeRateRequired ? Value * this.ActualRate() - StorageToCommission : Value - StorageToCommission;
            set
            {
                // take into account currency exchange rate and commission
                if (IsCurrencyExchangeRateRequired && this.ActualRate() != 0)
                    Value = value / this.ActualRate() + StorageToCommission;
                else
                    Value = value + StorageToCommission;
            }
        }

        public string ValueTransfering => $"{StorageFromCurrency.FormatValue(StorageFromValue)} -> {StorageToCurrency.FormatValue(StorageToValue)}";

        #endregion
    }
}
