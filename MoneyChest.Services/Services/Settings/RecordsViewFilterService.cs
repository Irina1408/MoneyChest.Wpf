using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Entities;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using System.Linq.Expressions;

namespace MoneyChest.Services.Services.Settings
{
    public interface IRecordsViewFilterService : IBaseUserableService<RecordsViewFilter>
    {
    }

    public class RecordsViewFilterService : BaseUserableService<RecordsViewFilter>, IRecordsViewFilterService
    {
        public RecordsViewFilterService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(RecordsViewFilter entity) => entity.UserId;

        protected override Expression<Func<RecordsViewFilter, bool>> LimitByUser(int userId) => item => item.UserId == userId;
    }
}
