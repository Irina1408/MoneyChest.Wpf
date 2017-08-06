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
    public interface IWeeklyScheduleService : IBaseHistoricizedService<WeeklySchedule>
    {
    }

    public class WeeklyScheduleService : BaseHistoricizedService<WeeklySchedule>, IWeeklyScheduleService
    {
        public WeeklyScheduleService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(WeeklySchedule entity)
        {
            if (entity.Event != null) return entity.Event.UserId;
            return _context.Events.FirstOrDefault(_ => _.Id == entity.EventId).UserId;
        }

        protected override Expression<Func<WeeklySchedule, bool>> LimitByUser(int userId) => item => item.Event.UserId == userId;
    }
}
