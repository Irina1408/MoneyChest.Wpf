using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using MoneyChest.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Builders.Base
{
    public abstract class DataBuilderBase<TSettings, TResultItem> : IDataBuilder<TSettings, TResultItem>
    {
        #region Protected fields

        protected int _userId;
        protected bool _isDataLoaded;

        protected IRecordService _recordService;
        protected ICurrencyService _currencyService;
        protected ICurrencyExchangeRateService _currencyExchangeRateService;

        protected CurrencyModel _mainCurrency;
        protected List<CurrencyExchangeRateModel> _currencyExchangeRates;

        #endregion

        #region Initialization

        public DataBuilderBase(int userId,
            IRecordService recordService,
            ICurrencyService currencyService,
            ICurrencyExchangeRateService currencyExchangeRateService)
        {
            _userId = userId;
            _isDataLoaded = false;

            _recordService = recordService;
            _currencyService = currencyService;
            _currencyExchangeRateService = currencyExchangeRateService;
        }

        #endregion

        #region IDataBuilder<TSettings, TResultItem> implementation 

        public virtual List<TResultItem> Build(TSettings settings)
        {
            // pre-load data
            if (!_isDataLoaded)
                LoadData();

            return BuildResult(settings);
        }

        #endregion

        #region Protected methods

        protected abstract List<TResultItem> BuildResult(TSettings settings);

        protected virtual void LoadData()
        {
            _mainCurrency = _currencyService.GetMain(_userId);
            _currencyExchangeRates = _currencyExchangeRateService.GetList(_userId, _mainCurrency.Id);
            _isDataLoaded = true;
        }

        #endregion
    }
}
