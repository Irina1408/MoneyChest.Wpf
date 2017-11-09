using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using MoneyChest.Model.Base;
using MoneyChest.Data.Converters;

namespace MoneyChest.Services.Services.Base
{
    public abstract class HistoricizedIdManageableUserableListServiceBase<T, TModel, TConverter> : HistoricizedIdManageableServiceBase<T, TModel, TConverter>, IIdManagableUserableListServiceBase<TModel>
        where T : class, IHasId, IHasUserId
        where TModel : class, IHasId
        where TConverter : IEntityModelConverter<T, TModel>, new()
    {
        public HistoricizedIdManageableUserableListServiceBase(ApplicationDbContext context) : base(context)
        {
        }

        public List<TModel> GetListForUser(int userId) => Scope.Where(e => e.UserId == userId).ToList().ConvertAll(_converter.ToModel);

        protected override int UserId(T entity) => entity.UserId;
    }
}
