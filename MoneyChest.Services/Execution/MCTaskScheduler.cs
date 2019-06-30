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
        private ITransactionService _service;

        #endregion

        #region Public methods

        public void Start(int userId)
        {
            // init user
            _userId = userId;
            _service = ServiceManager.ConfigureService<TransactionService>();

            // execute all planned transactions with autoexecution


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
            IEventService eventService = ServiceManager.ConfigureService<EventService>();
            eventService.ExecuteEvents(_userId);
        }

        #endregion
    }
}
