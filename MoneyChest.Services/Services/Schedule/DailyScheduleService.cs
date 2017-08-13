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
    public interface IDailyScheduleService : IBaseHistoricizedService<DailySchedule>, IIdManageable<DailySchedule>
    {
    }

    public class DailyScheduleService : BaseHistoricizedService<DailySchedule>, IDailyScheduleService
    {
        public DailyScheduleService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(DailySchedule entity)
        {
            if (entity.Event != null) return entity.Event.UserId;
            return _context.Events.FirstOrDefault(_ => _.Id == entity.EventId).UserId;
        }

        protected override Expression<Func<DailySchedule, bool>> LimitByUser(int userId) => item => item.Event.UserId == userId;

        #region IIdManageable<T> implementation

        public DailySchedule Get(int id) => Entities.FirstOrDefault(_ => _.Id == id);

        public List<DailySchedule> Get(List<int> ids) => Entities.Where(_ => ids.Contains(_.Id)).ToList();

        public void Delete(int id) => Delete(Get(id));

        #endregion
    }
}
