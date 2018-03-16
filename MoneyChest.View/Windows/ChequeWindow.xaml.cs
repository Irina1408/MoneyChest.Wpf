﻿using MahApps.Metro.Controls;
using MoneyChest.Model.Enums;
using MoneyChest.Model.Extensions;
using MoneyChest.Model.Model;
using MoneyChest.Services;
using MoneyChest.Services.Services;
using MoneyChest.Shared;
using MoneyChest.Shared.MultiLang;
using MoneyChest.View.Utils;
using MoneyChest.ViewModel.Commands;
using MoneyChest.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MoneyChest.View.Windows
{
    /// <summary>
    /// Interaction logic for ChequeWindow.xaml
    /// </summary>
    public partial class ChequeWindow : MetroWindow
    {
        #region Private fields

        private ChequeViewModel _viewModel;
        private IRecordService _service;
        private IEnumerable<CurrencyModel> _currencies;
        private IEnumerable<StorageModel> _storages;

        #endregion

        #region Initialization

        public ChequeWindow()
        {
            InitializeComponent();

            // init
            _service = ServiceManager.ConfigureService<RecordService>();

            // load storages
            IStorageService storageService = ServiceManager.ConfigureService<StorageService>();
            _storages = storageService.GetVisible(GlobalVariables.UserId);
            comboStorage.ItemsSource = _storages;

            // load currencies
            ICurrencyService currencyService = ServiceManager.ConfigureService<CurrencyService>();
            _currencies = currencyService.GetActive(GlobalVariables.UserId);
            comboCurrencies.ItemsSource = _currencies;

            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            _viewModel = new ChequeViewModel();

            _viewModel.AddCommand = new Command(() => _viewModel.Entities.Add(new RecordModel()
            {
                UserId = GlobalVariables.UserId,
                RecordType = _viewModel.Entities.Count > 0 ? _viewModel.Entities.Last().RecordType : RecordType.Expense
            }));

            _viewModel.DeleteCommand = new DataGridSelectedItemsCommand<RecordModel>(GridRecords, (items) =>
            {
                // remove in only grid
                foreach (var item in items.ToList())
                    _viewModel.Entities.Remove(item);
            });

            _viewModel.SaveCommand = new Command(() =>
            {
                // check valid data
                if(!ValidateData())
                {
                    MessageBox.Show(MultiLangResourceManager.Instance[MultiLangResourceName.SaveFailedMessage],
                        MultiLangResourceManager.Instance[MultiLangResourceName.SaveFailed], MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);

                    return;
                }

                foreach(var entity in _viewModel.Entities)
                {
                    entity.Date = _viewModel.Date;
                    entity.CurrencyId = _viewModel.CurrencyId;
                    entity.StorageId = _viewModel.StorageId;
                    entity.Storage = _viewModel.Storage;
                    entity.CurrencyExchangeRate = _viewModel.CurrencyExchangeRate;
                    if (string.IsNullOrEmpty(entity.Description)) entity.Description = _viewModel.Description;
                    if (string.IsNullOrEmpty(entity.Remark)) entity.Remark = _viewModel.Remark;
                    _service.Add(entity);
                }

                _viewModel.Entities.Clear();
                DialogResult = true;
                this.Close();
            });

            _viewModel.CancelCommand = new Command(() =>
            {
                DialogResult = false;
                this.Close();
            });

            // init columns
            _viewModel.RecordTypeList = MultiLangEnumHelper.ToCollection(typeof(RecordType));

            // fill defaults
            _viewModel.Date = DateTime.Now;
            _viewModel.CurrencyId = _currencies.FirstOrDefault(x => x.IsMain)?.Id ?? _currencies.FirstOrDefault()?.Id ?? 0;
            _viewModel.StorageId = _storages.FirstOrDefault(x => x.CurrencyId == _viewModel.CurrencyId)?.Id;
            //_viewModel.Storage = _storages.FirstOrDefault(x => x.CurrencyId == _viewModel.CurrencyId)?.ToReferenceView();

            this.DataContext = _viewModel;
        }

        #endregion

        #region Event handlers

        private void comboStorage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // update storage reference
            _viewModel.Storage = _storages.FirstOrDefault(x => x.Id == _viewModel.StorageId)?.ToReferenceView();
        }

        private void ChequeWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_viewModel.Entities.Count > 0)
            {
                // show confirmation
                var dialogResult = MessageBox.Show(MultiLangResourceManager.Instance[MultiLangResourceName.SaveChangesConfirmationMessage],
                    MultiLangResourceManager.Instance[MultiLangResourceName.SaveChangesConfirmation], MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Exclamation, MessageBoxResult.Yes);

                if (dialogResult == MessageBoxResult.Yes)
                {
                    // check valid data
                    if (!ValidateData())
                    {
                        MessageBox.Show(MultiLangResourceManager.Instance[MultiLangResourceName.SaveFailedMessage],
                            MultiLangResourceManager.Instance[MultiLangResourceName.SaveFailed], MessageBoxButton.OK,
                            MessageBoxImage.Exclamation);

                        e.Cancel = true;
                    }
                }
            }
        }

        #endregion

        #region Private methods

        private bool ValidateData()
        {
            // check currency
            return _viewModel.CurrencyId > 0;
        }

        #endregion
    }
}
