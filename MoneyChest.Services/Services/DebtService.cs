using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;

namespace MoneyChest.Services.Services
{
    public interface IDebtService : IBaseHistoricizedService<Debt>
    {
    }

    public class DebtService : BaseHistoricizedService<Debt>, IDebtService
    {
        public DebtService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(Debt entity) => entity.UserId;

        public override Func<Debt, bool> LimitByUser(int userId) => item => item.UserId == userId;
    }
}
