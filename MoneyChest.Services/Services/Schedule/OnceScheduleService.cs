using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using System.Linq.Expressions;
using MoneyChest.Model.Model;
using MoneyChest.Data.Converters;

namespace MoneyChest.Services.Services
{
    public interface IOnceScheduleService : IIdManagableServiceBase<OnceScheduleModel>
    {
    }

    public class OnceScheduleService : HistoricizedIdManageableServiceBase<OnceSchedule, OnceScheduleModel, OnceScheduleConverter>, IOnceScheduleService
    {
        public OnceScheduleService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(OnceSchedule entity)
        {
            if (entity.Event != null) return entity.Event.UserId;
            return _context.Events.FirstOrDefault(_ => _.Id == entity.EventId).UserId;
        }
        
    }
}
