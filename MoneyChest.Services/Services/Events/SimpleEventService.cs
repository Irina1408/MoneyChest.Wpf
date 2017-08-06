using MoneyChest.Data.Entities;
using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;

namespace MoneyChest.Services.Services.Events
{
    public interface ISimpleEventService : IBaseHistoricizedService<SimpleEvent>
    {
    }

    public class SimpleEventService : BaseHistoricizedService<SimpleEvent>, ISimpleEventService
    {
        public SimpleEventService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(SimpleEvent entity) => entity.UserId;
    }
}
