using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using MoneyChest.Model.Base;
using MoneyChest.Services.Converters;

namespace MoneyChest.Services.Services.Base
{
    public abstract class HistoricizedIdManageableServiceBase<T, TModel, TConverter> : HistoricizedServiceBase<T, TModel, TConverter>, IIdManagableServiceBase<TModel>
        where T : class, IHasId
        where TModel : class, IHasId
        where TConverter : IEntityModelConverter<T, TModel>, new()
    {
        public HistoricizedIdManageableServiceBase(ApplicationDbContext context) : base(context)
        {
        }

        public TModel Get(int id)
        {
            var entity = Scope.FirstOrDefault(e => e.Id == id);
            return entity != null ? _converter.ToModel(entity) : null;
        }

        public List<TModel> Get(List<int> ids) => Scope.Where(e => ids.Contains(e.Id)).ToList().ConvertAll(_converter.ToModel);

        public void Delete(int id) => Delete(Entities.FirstOrDefault(e => e.Id == id));

        protected override T GetDbEntity(TModel model) => Entities.FirstOrDefault(e => e.Id == model.Id);
        protected override T GetDbDetailedEntity(T entity) => Scope.FirstOrDefault(e => e.Id == entity.Id);
        protected override List<T> GetDbEntities(IEnumerable<TModel> models)
        {
            var ids = models.Select(_ => _.Id).ToList();
            return Entities.Where(e => ids.Contains(e.Id)).ToList();
        }
    }
}
