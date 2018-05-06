using MoneyChest.Data.Entities;
using MoneyChest.Data.Enums;
using MoneyChest.Data.Mock.Utils;
using MoneyChest.Model.Enums;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Mock
{
    public partial class DbFactoryDefinitions
    {
        public static void Define(DbFactory f)
        {
            DefineEntities(f);
            DefineModels(f);
        }

        private static void DefineEntities(DbFactory f)
        {
            f.Define<User>(e =>
            {
                e.Name = Moniker.UserName;
                e.Password = Moniker.UserPassword;
                e.LastUsageDate = DateTime.Now;
            });

            f.Define<Category>(e =>
            {
                e.Name = Moniker.Category;
                e.IsActive = true;
            });

            f.Define<Currency>(e =>
            {
                e.Name = Moniker.Currency;
                e.Code = Moniker.CurrencyCode;
                e.Symbol = Moniker.CurrencySymbol;
                e.IsActive = true;
                e.IsMain = false;
                e.CurrencySymbolAlignment = CurrencySymbolAlignment.Right;
            });

            f.Define<CurrencyExchangeRate>(e =>
            {
                e.Rate = Moniker.LimitedDigit(30);
            });

            f.Define<StorageGroup>(e =>
            {
                e.Name = Moniker.StorageGroup;
            });

            f.Define<Storage>(e =>
            {
                e.Name = "Storage";
                e.Value = Moniker.Digit;
                e.IsVisible = true;
            });

            f.Define<Debt>(e =>
            {
                e.Description = "Take Debt";
                e.DebtType = DebtType.TakeBorrow;
                e.TakingDate = DateTime.Today;
                e.Value = Moniker.Digit;
                e.IsRepaid = false;
            });

            f.Define<Record>(e =>
            {
                e.Description = "Record";
                e.RecordType = RecordType.Expense;
                e.Date = DateTime.Now;
                e.Value = Moniker.Digit;
            });

            f.Define<MoneyTransfer>(e =>
            {
                e.Date = DateTime.Now;
                e.Description = "MoneyTransfer";
                e.Value = Moniker.Digit;
                e.CurrencyExchangeRate = 1;
                e.TakeCommissionFromReceiver = false;
                e.CommissionType = CommissionType.Currency;
            });

            f.Define<Limit>(e =>
            {
                e.Value = Moniker.Digit;
                e.Description = "Limit";
                e.DateFrom = DateTime.Today.AddDays(1);
                e.DateUntil = e.DateFrom.Date;
            });

            DefineEventEntities(f);
            DefineSettingsEntities(f);
        }

        private static void DefineEventEntities(DbFactory f)
        {

            f.Define<MoneyTransferEvent>(e =>
            {
                e.Description = "MoneyTransferEvent";
                e.Value = Moniker.Digit;
                e.EventState = EventState.Active;
                e.AutoExecution = false;
                e.ConfirmBeforeExecute = false;
            });

            f.Define<RepayDebtEvent>(e =>
            {
                e.Description = "RepayDebtEvent";
                e.Value = Moniker.Digit;
                e.EventState = EventState.Active;
                e.AutoExecution = false;
                e.ConfirmBeforeExecute = false;
            });

            f.Define<SimpleEvent>(e =>
            {
                e.Description = "RepayDebtEvent";
                e.Value = Moniker.Digit;
                e.EventState = EventState.Active;
                e.AutoExecution = false;
                e.ConfirmBeforeExecute = false;
                e.RecordType = RecordType.Expense;
            });
        }

        private static void DefineSettingsEntities(DbFactory f)
        {
            f.Define<CalendarSettings>(e =>
            {
                //e.PeriodType = CalendarPeriodType.Month;
                e.ShowLimits = false;
            });

            f.Define<ForecastSetting>(e =>
            {
                e.AllCategories = true;
                e.RepeatsCount = 5;
                e.ActualDays = 100;
            });

            f.Define<GeneralSetting>(e =>
            {
                e.Language = Language.English;
                e.FirstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            });

            //f.Define<TransactionsSettings>(e =>
            //{
            //    e.AllCategories = true;
            //    e.PeriodFilterType = PeriodFilterType.ThisMonth;
            //});

            f.Define<ReportSetting>(e =>
            {
                e.ReportType = ReportType.PieChart;
                e.CategoryLevel = -1;
            });
        }

        private static void DefineModels(DbFactory f)
        {
            f.Define<UserModel>(e =>
            {
                e.Name = Moniker.UserName;
                e.Password = Moniker.UserPassword;
            });

            f.Define<CategoryModel>(e =>
            {
                e.Name = Moniker.Category;
            });

            f.Define<CurrencyModel>(e =>
            {
                e.Name = Moniker.Currency;
                e.Code = Moniker.CurrencyCode;
                e.Symbol = Moniker.CurrencySymbol;
                e.IsActive = true;
                e.IsMain = false;
            });

            f.Define<StorageGroupModel>(e =>
            {
                e.Name = Moniker.StorageGroup;
            });

            f.Define<StorageModel>(e =>
            {
                e.Name = "Storage";
                e.Value = Moniker.Digit;
            });

            f.Define<CurrencyExchangeRateModel>(e =>
            {
                e.Rate = Moniker.LimitedDigit(30);
            });

            f.Define<DebtModel>(e =>
            {
                e.Description = "Take Debt";
                e.DebtType = DebtType.TakeBorrow;
                e.TakingDate = DateTime.Today;
                e.Value = Moniker.Digit;
            });

            f.Define<RecordModel>(e =>
            {
                e.Description = "Record";
                e.RecordType = RecordType.Expense;
                e.Date = DateTime.Now;
                e.Value = Moniker.Digit;
            });

            f.Define<LimitModel>(e =>
            {
                e.Value = Moniker.Digit;
            });

            f.Define<MoneyTransferEventModel>(e =>
            {
                e.Description = "MoneyTransferEvent";
                e.Value = Moniker.Digit;
                e.EventState = EventState.Active;
                e.AutoExecution = false;
                e.ConfirmBeforeExecute = false;
            });

            f.Define<RepayDebtEventModel>(e =>
            {
                e.Description = "RepayDebtEvent";
                e.Value = Moniker.Digit;
                e.EventState = EventState.Active;
                e.AutoExecution = false;
                e.ConfirmBeforeExecute = false;
            });

            f.Define<SimpleEventModel>(e =>
            {
                e.Description = "RepayDebtEvent";
                e.Value = Moniker.Digit;
                e.EventState = EventState.Active;
                e.AutoExecution = false;
                e.ConfirmBeforeExecute = false;
                e.RecordType = RecordType.Expense;
            });
        }
    }
}
