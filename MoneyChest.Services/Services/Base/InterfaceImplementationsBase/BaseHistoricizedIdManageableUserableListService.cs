using MoneyChest.Data.Entities.Base;
using MoneyChest.Model.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;

namespace MoneyChest.Services.Services.Base
{
    public abstract class BaseHistoricizedIdManageableUserableListService<T, TModel, TConverter> : BaseHistoricizedIdManageableService<T, TModel, TConverter>, IBaseIdManagableUserableListService<TModel>
        where T : class, IHasId, IHasUserId
        where TModel : class, IHasId
        where TConverter : IEntityModelConverter<T, TModel>, new()
    {
        public BaseHistoricizedIdManageableUserableListService(ApplicationDbContext context) : base(context)
        {
        }

        public List<TModel> GetListForUser(int userId) => Scope.Where(e => e.UserId == userId).ToList().ConvertAll(_converter.ToModel);

        protected override int UserId(T entity) => entity.UserId;
    }
}
