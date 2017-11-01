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
using MoneyChest.Model.Extensions;
using System.Data.Entity;
using MoneyChest.Data.Converters;

namespace MoneyChest.Services.Services
{
    public interface ILimitService : IBaseIdManagableUserableListService<LimitModel>
    {
    }

    public class LimitService : BaseHistoricizedIdManageableUserableListService<Limit, LimitModel, LimitConverter>, ILimitService
    {
        public LimitService(ApplicationDbContext context) : base(context)
        {
        }

        protected override IQueryable<Limit> Scope => Entities.Include(_ => _.Currency).Include(_ => _.Category);
    }
}
