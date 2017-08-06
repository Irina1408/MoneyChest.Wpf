using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using System.Linq.Expressions;

namespace MoneyChest.Services.Services.Schedule
{
    public interface IMonthlyScheduleService : IBaseHistoricizedService<MonthlySchedule>, IIdManageable<MonthlySchedule>
    {
    }

    public class MonthlyScheduleService : BaseHistoricizedService<MonthlySchedule>, IMonthlyScheduleService
    {
        public MonthlyScheduleService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(MonthlySchedule entity)
        {
            if (entity.Event != null) return entity.Event.UserId;
            return _context.Events.FirstOrDefault(_ => _.Id == entity.EventId).UserId;
        }

        protected override Expression<Func<MonthlySchedule, bool>> LimitByUser(int userId) => item => item.Event.UserId == userId;

        #region IIdManageable<T> implementation

        public MonthlySchedule Get(int id) => Entities.FirstOrDefault(_ => _.Id == id);

        public void Delete(int id) => Delete(Get(id));

        #endregion
    }
}
