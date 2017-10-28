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
using MoneyChest.Model.Model;
using System.Data.Entity;
using MoneyChest.Model.Convert;
using MoneyChest.Model.Converters;

namespace MoneyChest.Services.Services
{
    public interface IRecordService : IBaseIdManagableUserableListService<RecordModel>
    {
        List<RecordModel> Get(int userId, PeriodFilterType period, TransactionType transactionType, bool includeWithoutCategory, List<int> categoryIds = null);
        List<RecordModel> Get(int userId, DateTime from, DateTime until, TransactionType transactionType, bool includeWithoutCategory, List<int> categoryIds = null);
        List<RecordModel> Get(int userId, DateTime from, DateTime until, List<int> storageGroupIds);
    }

    public class RecordService : BaseHistoricizedIdManageableUserableListService<Record, RecordModel, RecordConverter>, IRecordService
    {
        #region Initialization

        public RecordService(ApplicationDbContext context) : base(context)
        {
        }

        #endregion

        #region IRecordService implementation

        public List<RecordModel> Get(int userId, PeriodFilterType period, TransactionType transactionType, bool includeWithoutCategory, List<int> categoryIds = null)
        {
            if (period == PeriodFilterType.All || period == PeriodFilterType.CustomPeriod)
                // all records for transaction type and categories
                if(categoryIds == null)
                    return Scope.Where(item => item.UserId == userId && item.TransactionType == transactionType
                         && (includeWithoutCategory && item.CategoryId == null || item.CategoryId != null)).ToList().ConvertAll(_converter.ToModel);
                else
                    return Scope.Where(item => item.UserId == userId && item.TransactionType == transactionType
                         && (includeWithoutCategory && item.CategoryId == null
                        || (item.CategoryId != null && categoryIds.Contains((int)item.CategoryId)))).ToList().ConvertAll(_converter.ToModel);
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

        public List<RecordModel> Get(int userId, DateTime from, DateTime until, TransactionType transactionType, bool includeWithoutCategory, List<int> categoryIds = null)
        {
            if(categoryIds == null)
                return Scope.Where(item => item.UserId == userId && item.TransactionType == transactionType
                    && item.Date >= from && item.Date <= until
                    && (includeWithoutCategory && item.CategoryId == null || item.CategoryId != null)).ToList().ConvertAll(_converter.ToModel);
            else
                return Scope.Where(item => item.UserId == userId && item.TransactionType == transactionType
                    && item.Date >= from && item.Date <= until
                    && (includeWithoutCategory && item.CategoryId == null
                    || (item.CategoryId != null && categoryIds.Contains((int)item.CategoryId)))).ToList().ConvertAll(_converter.ToModel);
        }

        public List<RecordModel> Get(int userId, DateTime from, DateTime until, List<int> storageGroupIds)
        {
            return Scope.Where(item => item.UserId == userId && item.Date >= from && item.Date <= until
                && (!item.StorageId.HasValue || storageGroupIds.Contains(item.StorageId.Value))).ToList().ConvertAll(_converter.ToModel);
        }

        #endregion

        #region Overrides

        protected override IQueryable<Record> Scope => Entities.Include(_ => _.Currency).Include(_ => _.Category).Include(_ => _.Storage).Include(_ => _.Debt);

        #endregion
    }
}
