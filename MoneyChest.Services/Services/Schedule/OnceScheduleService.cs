﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;

namespace MoneyChest.Services.Services.Schedule
{
    public interface IOnceScheduleService : IBaseHistoricizedService<OnceSchedule>
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
    }
}
