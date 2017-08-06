using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using System.Linq.Expressions;

namespace MoneyChest.Services.Services
{
    public interface ILimitService : IBaseHistoricizedService<Limit>, IIdManageable<Limit>
    {
    }

    public class LimitService : BaseHistoricizedService<Limit>, ILimitService
    {
        public LimitService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(Limit entity) => entity.UserId;

        protected override Expression<Func<Limit, bool>> LimitByUser(int userId) => item => item.UserId == userId;

        #region IIdManageable<T> implementation

        public Limit Get(int id) => Entities.FirstOrDefault(_ => _.Id == id);

        public void Delete(int id) => Delete(Get(id));

        #endregion
    }
}
