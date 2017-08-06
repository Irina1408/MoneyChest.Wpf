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
    public interface ILimitService : IBaseHistoricizedService<Limit>
    {
    }

    public class LimitService : BaseHistoricizedService<Limit>, ILimitService
    {
        public LimitService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(Limit entity) => entity.UserId;

        public override Func<Limit, bool> LimitByUser(int userId) => item => item.UserId == userId;
    }
}
