﻿using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class DataFilterModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandler OnFilterChanged;

        #region Private fields
        
        private bool _isPopulation;
        private bool _isFilterChanged;

        #endregion

        #region Initialization

        public DataFilterModel()
        {
            CategoryIds = new List<int>();
            StorageIds = new List<int>();
            _isFilterChanged = false;
            _isPopulation = false;

            PropertyChanged += (sender, e) =>
            {
                // skip additional properties
                if (e.PropertyName == nameof(IsPopulation)
                    || e.PropertyName == nameof(IsDataFiltered)
                    || e.PropertyName == nameof(IncludeWithoutCategory)
                    || e.PropertyName == nameof(AllCategories))
                    return;

                if (!_isPopulation)
                    NotifyFilterChanged(e.PropertyName);
                else
                    _isFilterChanged = true;
            };
        }

        #endregion

        #region Public properties

        public bool IsPopulation
        {
            get => _isPopulation;
            set
            {
                _isPopulation = value;
                if (!_isPopulation && _isFilterChanged)
                    NotifyFilterChanged();
            }
        }
        
        public bool IsFilterVisible { get; set; }
        public bool IsFilterApplied { get; set; }
        public bool IsCategoryBranchSelection { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
        public TransactionType? TransactionType { get; set; }
        
        public List<int> CategoryIds { get; set; }
        public List<int> StorageIds { get; set; }

        public bool IsTransactionTypeFiltered
        {
            get => TransactionType != null;
            set => TransactionType = value ? (TransactionType?)Enums.TransactionType.Expense : null;
        }

        public bool IsDataFiltered => StorageIds.Count > 0 || CategoryIds.Count > 0 || TransactionType != null 
            || !string.IsNullOrEmpty(Description) || !string.IsNullOrEmpty(Remark);

        public bool IncludeWithoutCategory => CategoryIds.Count == 0 || CategoryIds.Contains(-1);
        public bool AllCategories => CategoryIds.Count == 0;

        #endregion

        #region Private methods

        private void NotifyFilterChanged(string propertyName = null)
        {
            // notify filter was changed
            OnFilterChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            // set filter changing is notified
            _isFilterChanged = false;
        }

        #endregion
    }
}
