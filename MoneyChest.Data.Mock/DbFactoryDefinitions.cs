using MoneyChest.Data.Entities;
using MoneyChest.Data.Enums;
using MoneyChest.Data.Mock.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Mock
{
    public partial class DbFactoryDefinitions
    {
        public static void Define(DbFactory f)
        {
            f.Define<User>(e =>
            {
                e.Name = Moniker.UserName;
                e.Password = Moniker.UserPassword;
            });

            f.Define<Category>(e =>
            {
                e.Name = Moniker.Category;
            });

            f.Define<Currency>(e =>
            {
                e.Name = Moniker.Currency;
                e.Code = Moniker.CurrencyCode;
                e.Symbol = Moniker.CurrencySymbol;
                e.IsUsed = true;
                e.IsMain = false;
            });

            f.Define<StorageGroup>(e =>
            {
                e.Name = Moniker.StorageGroup;
            });

            f.Define<Storage>(e =>
            {
                e.Name = "Storage";
                e.Value = Moniker.Digit;
            });

            f.Define<CurrencyExchangeRate>(e =>
            {
                e.Rate = Moniker.LimitedDigit(20);
            });
            
            f.Define<Debt>(e =>
            {
                e.Name = "Take Debt";
                e.DebtType = DebtType.TakeBorrow;
                e.TakingDate = DateTime.Today;
                e.Value = Moniker.Digit;
            });

            f.Define<Record>(e =>
            {
                e.Description = "Record";
                e.TransactionType = TransactionType.Expense;
                e.Date = DateTime.Now;
                e.Value = Moniker.Digit;
            });

            f.Define<Limit>(e =>
            {
                e.Value = Moniker.Digit;
            });

            f.Define<MoneyTransferEvent>(e =>
            {
                e.Description = "MoneyTransferEvent";
                e.Value = Moniker.Digit;
                e.EventState = Enums.EventState.Active;
                e.AutoExecution = false;
                e.ConfirmBeforeExecute = false;
                e.ScheduleType = Enums.ScheduleType.Once;
            });

            f.Define<RepayDebtEvent>(e =>
            {
                e.Description = "RepayDebtEvent";
                e.Value = Moniker.Digit;
                e.EventState = EventState.Active;
                e.AutoExecution = false;
                e.ConfirmBeforeExecute = false;
                e.ScheduleType = ScheduleType.Once;
            });

            f.Define<SimpleEvent>(e =>
            {
                e.Description = "RepayDebtEvent";
                e.Value = Moniker.Digit;
                e.EventState = EventState.Active;
                e.AutoExecution = false;
                e.ConfirmBeforeExecute = false;
                e.ScheduleType = ScheduleType.Once;
                e.TransactionType = TransactionType.Expense;
            });
        }
    }
}
