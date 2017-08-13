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
    public interface IOnceScheduleService : IBaseHistoricizedService<OnceSchedule>, IIdManageable<OnceSchedule>
    {
    }

    public class OnceScheduleService : BaseHistoricizedService<OnceSchedule>, IOnceScheduleService
    {
        public OnceScheduleService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(OnceSchedule entity)
        {
            if (entity.Event != null) return entity.Event.UserId;
            return _context.Events.FirstOrDefault(_ => _.Id == entity.EventId).UserId;
        }

        protected override Expression<Func<OnceSchedule, bool>> LimitByUser(int userId) => item => item.Event.UserId == userId;

        #region IIdManageable<T> implementation

        public OnceSchedule Get(int id) => Entities.FirstOrDefault(_ => _.Id == id);

        public List<OnceSchedule> Get(List<int> ids) => Entities.Where(_ => ids.Contains(_.Id)).ToList();

        public void Delete(int id) => Delete(Get(id));

        #endregion
    }
}
