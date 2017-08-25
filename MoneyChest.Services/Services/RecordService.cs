using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using System.Linq.Expressions;
using MoneyChest.Data.Enums;
using MoneyChest.Services.Utils;

namespace MoneyChest.Services.Services
{
    public interface IRecordService : IBaseHistoricizedService<Record>, IIdManageable<Record>
    {
        List<Record> Get(int userId, PeriodFilterType period, TransactionType transactionType, bool includeWithoutCategory, List<int> categoryIds = null);
        List<Record> Get(int userId, DateTime from, DateTime until, TransactionType transactionType, bool includeWithoutCategory, List<int> categoryIds = null);
    }

    public class RecordService : BaseHistoricizedService<Record>, IRecordService
    {
        #region Initialization

        public RecordService(ApplicationDbContext context) : base(context)
        {
        }

        #endregion

        #region IRecordService implementation

        public List<Record> Get(int userId, PeriodFilterType period, TransactionType transactionType, bool includeWithoutCategory, List<int> categoryIds = null)
        {
            if (period == PeriodFilterType.All || period == PeriodFilterType.CustomPeriod)
                // all records for transaction type and categories
                return GetAllForUser(userId,
                    item => item.TransactionType == transactionType
                    && (includeWithoutCategory && item.CategoryId == null
                    || (item.CategoryId != null && (categoryIds == null || categoryIds.Contains((int)item.CategoryId)))));
            else
            {
                // get general settings for getting first day of week
                var generalSettings = _context.GeneralSettings.FirstOrDefault(item => item.UserId == userId);
                // get period
                var p = ServiceHelper.GetPeriod(period, generalSettings.FirstDayOfWeek);
                // return result
                return Get(userId, p.Item1, p.Item2, transactionType, includeWithoutCategory, categoryIds);
            }
        }

        public List<Record> Get(int userId, DateTime from, DateTime until, TransactionType transactionType, bool includeWithoutCategory, List<int> categoryIds = null)
        {
            if(categoryIds == null)
                return GetAllForUser(userId, item => item.TransactionType == transactionType
                    && item.Date >= from && item.Date <= until
                    && (includeWithoutCategory && item.CategoryId == null || item.CategoryId != null));
            else
                return GetAllForUser(userId, item => item.TransactionType == transactionType
                    && item.Date >= from && item.Date <= until
                    && (includeWithoutCategory && item.CategoryId == null
                    || (item.CategoryId != null && categoryIds.Contains((int)item.CategoryId))));
        }

        #endregion

        #region Overrides

        protected override int UserId(Record entity) => entity.UserId;

        protected override Expression<Func<Record, bool>> LimitByUser(int userId) => item => item.UserId == userId;

        #endregion

        #region IIdManageable<T> implementation

        public Record Get(int id) => Entities.FirstOrDefault(_ => _.Id == id);

        public List<Record> Get(List<int> ids) => Entities.Where(_ => ids.Contains(_.Id)).ToList();

        public void Delete(int id) => Delete(Get(id));

        #endregion
    }
}
