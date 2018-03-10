using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using MoneyChest.Model.Model;

namespace MoneyChest.Services.Services
{
    public interface ITransactionService
    {
        List<ITransaction> Get(int userId, DateTime dateFrom, DateTime dateUntil);
        void Delete(IEnumerable<ITransaction> entities);
    }

    public class TransactionService : ServiceBase, ITransactionService
    {
        #region Private fields

        private IRecordService _recordService;
        private IMoneyTransferService _moneyTransferService;
        private ISimpleEventService _simpleEventService;
        private IMoneyTransferEventService _moneyTransferEventService;
        private IRepayDebtEventService _repayDebtEventService;

        #endregion

        #region Initialization

        public TransactionService(ApplicationDbContext context) : base(context)
        {
            _recordService = new RecordService(context);
            _moneyTransferService = new MoneyTransferService(context);
            _simpleEventService = new SimpleEventService(context);
            _moneyTransferEventService = new MoneyTransferEventService(context);
            _repayDebtEventService = new RepayDebtEventService(context);
        }

        #endregion

        #region ITransactionService implementation

        public List<ITransaction> Get(int userId, DateTime dateFrom, DateTime dateUntil)
        {
            var result = new List<ITransaction>();

            // load records
            result.AddRange(_recordService.Get(userId, dateFrom, dateUntil));
            // load money transfers
            result.AddRange(_moneyTransferService.Get(userId, dateFrom, dateUntil));
            
            // add events in case when selected period contains future dates
            if(dateUntil >= DateTime.Today)
            {
                // load all simple events 
                //var simpleEvents = simpleEventService.

            }

            return result.OrderByDescending(x => x.TransactionDate).ToList();
        }

        public void Delete(IEnumerable<ITransaction> entities)
        {
            _recordService.Delete(entities.Where(x => x is RecordModel).Select(x => x as RecordModel));
            _moneyTransferService.Delete(entities.Where(x => x is MoneyTransferModel).Select(x => x as MoneyTransferModel));
        }

        #endregion
    }
}
