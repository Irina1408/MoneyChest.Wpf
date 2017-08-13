using MoneyChest.Data.Entities;
using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using System.Linq.Expressions;

namespace MoneyChest.Services.Services.Events
{
    public interface ISimpleEventService : IBaseHistoricizedService<SimpleEvent>, IIdManageable<SimpleEvent>
    {
    }

    public class SimpleEventService : BaseHistoricizedService<SimpleEvent>, ISimpleEventService
    {
        public SimpleEventService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(SimpleEvent entity) => entity.UserId;

        protected override Expression<Func<SimpleEvent, bool>> LimitByUser(int userId) => item => item.UserId == userId;

        #region IIdManageable<T> implementation

        public SimpleEvent Get(int id) => Entities.FirstOrDefault(_ => _.Id == id);

        public List<SimpleEvent> Get(List<int> ids) => Entities.Where(_ => ids.Contains(_.Id)).ToList();

        public void Delete(int id) => Delete(Get(id));

        #endregion
    }
}
