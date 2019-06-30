using MoneyChest.Model.Model;
using MoneyChest.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Execution
{
    // TODO: use Tasks here
    public class MCTaskScheduler
    {
        #region Singleton

        private static MCTaskScheduler instance;
        public static MCTaskScheduler Instance => instance ?? (instance = new MCTaskScheduler());

        #endregion

        #region Private fields

        private int _userId;
        private DateTime _lastExecutionDate;
        private ITransactionService _service;

        #endregion

        #region Public methods

        public void Start(int userId, DateTime lastExecutionDate)
        {
            // init user
            _userId = userId;
            _lastExecutionDate = lastExecutionDate;
            _service = ServiceManager.ConfigureService<TransactionService>();

            // execute all planned transactions with autoexecution
            // TODO: replace in Task
            ExecuteEvents();

            //TODO: schedule execute events every day
        }

        public void End()
        {
            // TODO: close all tasks
            
        }

        #endregion

        #region Private methods
        
        private void ExecuteEvents()
        {
            if (_lastExecutionDate >= DateTime.Today) return;

            // fetch planned transactions
            var transactions = _service.GetPlanned(_userId, _lastExecutionDate.AddDays(1), DateTime.Today, false, true);
            // apply transactions
            _service.ExecutePlanned(transactions.Select(x => x as ITransaction), null, true);
            // update last execution date
            _lastExecutionDate = DateTime.Today;
        }

        #endregion
    }
}
