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
    public interface IRecordService : IBaseHistoricizedService<Record>
    {
    }

    public class RecordService : BaseHistoricizedService<Record>, IRecordService
    {
        public RecordService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(Record entity) => entity.UserId;

        protected override Expression<Func<Record, bool>> LimitByUser(int userId) => item => item.UserId == userId;
    }
}
