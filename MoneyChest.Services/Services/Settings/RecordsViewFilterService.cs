using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Entities;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;

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

        public override Func<RecordsViewFilter, bool> LimitByUser(int userId) => item => item.UserId == userId;
    }
}
