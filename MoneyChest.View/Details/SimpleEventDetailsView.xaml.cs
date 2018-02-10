﻿using MaterialDesignThemes.Wpf;
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
using MoneyChest.ViewModel.Wrappers;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoneyChest.View.Details
{
    public abstract class SimpleEventDetailsViewBase : BaseEntityDetailsView<SimpleEventModel, SimpleEventViewModel, ISimpleEventService>
    {
        public SimpleEventDetailsViewBase() : base()
        { }

        public SimpleEventDetailsViewBase(ISimpleEventService service, SimpleEventViewModel entity, bool isNew, Action closeAction)
            : base(service, entity, isNew, closeAction)
        { }
    }

    /// <summary>
    /// Interaction logic for SimpleEventDetailsView.xaml
    /// </summary>
    public partial class SimpleEventDetailsView : SimpleEventDetailsViewBase
    {
        #region Private fields

        private IEnumerable<CurrencyModel> _currencies;
        private IEnumerable<StorageModel> _storages;

        #endregion

        #region Initialization

        public SimpleEventDetailsView(ISimpleEventService service, SimpleEventViewModel entity, bool isNew, Action closeAction)
            : base(service, entity, isNew, closeAction)
        {
            InitializeComponent();
            
            // load storages
            IStorageService storageService = ServiceManager.ConfigureService<StorageService>();
            _storages = storageService.GetVisible(GlobalVariables.UserId, entity.StorageId);
            comboStorage.ItemsSource = _storages;

            // load currencies
            ICurrencyService currencyService = ServiceManager.ConfigureService<CurrencyService>();
            _currencies = currencyService.GetActive(GlobalVariables.UserId, entity.CurrencyId, entity.Storage?.CurrencyId);
            comboCurrencies.ItemsSource = _currencies;
            
            // set currencies list
            compCurrencyExchangeRate.CurrencyIds = 
                _storages.Select(_ => _.CurrencyId).Distinct().Concat(_currencies.Select(c => c.Id)).Distinct().ToList();

            // set header and commands panel context
            LabelHeader.Content = ViewHeader;
            CommandsPanel.DataContext = _commands;
        }

        #endregion

        #region Event handlers
        
        private void comboStorage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_wrappedEntity.IsChanged) return;
            _wrappedEntity.Entity.Storage = _storages.FirstOrDefault(_ => _.Id == _wrappedEntity.Entity.StorageId)?.ToReferenceView();
        }

        private void comboCurrencies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_wrappedEntity.IsChanged) return;
            _wrappedEntity.Entity.Currency = _currencies.FirstOrDefault(_ => _.Id == _wrappedEntity.Entity.CurrencyId)?.ToReferenceView();
        }

        #endregion
    }
}
