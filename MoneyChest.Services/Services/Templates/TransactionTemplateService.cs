using MoneyChest.Data.Context;
using MoneyChest.Model.Model;
using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Services
{
    public interface ITransactionTemplateService
    {
        List<ITransactionTemplate> Get(int userId);
        void Delete(IEnumerable<ITransactionTemplate> entities);
    }

    public class TransactionTemplateService : ServiceBase, ITransactionTemplateService
    {
        #region Private fields

        private IRecordTemplateService _recordTemplateService;
        private IMoneyTransferTemplateService _moneyTransferTemplateService;

        #endregion

        #region Initialization

        public TransactionTemplateService(ApplicationDbContext context) : base(context)
        {
            _recordTemplateService = new RecordTemplateService(context);
            _moneyTransferTemplateService = new MoneyTransferTemplateService(context);
        }

        #endregion

        #region ITransactionTemplateService implementation

        public List<ITransactionTemplate> Get(int userId)
        {
            var result = new List<ITransactionTemplate>();

            result.AddRange(_recordTemplateService.GetListForUser(userId));
            result.AddRange(_moneyTransferTemplateService.GetListForUser(userId));

            return result.OrderBy(x => x.Name).ToList();
        }

        public void Delete(IEnumerable<ITransactionTemplate> entities)
        {
            _recordTemplateService.Delete(entities.Where(x => x is RecordTemplateModel).Select(x => x as RecordTemplateModel));
            _moneyTransferTemplateService.Delete(entities.Where(x => x is MoneyTransferTemplateModel).Select(x => x as MoneyTransferTemplateModel));
        }

        #endregion
    }
}
