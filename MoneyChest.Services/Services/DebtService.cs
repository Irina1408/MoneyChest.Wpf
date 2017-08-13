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
    public interface IDebtService : IBaseHistoricizedService<Debt>, IIdManageable<Debt>
    {
    }

    public class DebtService : BaseHistoricizedService<Debt>, IDebtService
    {
        public DebtService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(Debt entity) => entity.UserId;

        protected override Expression<Func<Debt, bool>> LimitByUser(int userId) => item => item.UserId == userId;

        #region IIdManageable<T> implementation

        public Debt Get(int id) => Entities.FirstOrDefault(_ => _.Id == id);

        public List<Debt> Get(List<int> ids) => Entities.Where(_ => ids.Contains(_.Id)).ToList();

        public void Delete(int id) => Delete(Get(id));

        #endregion
    }
}
