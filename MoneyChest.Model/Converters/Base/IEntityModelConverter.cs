using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Converters
{
    public interface IEntityModelConverter<TEntity, TModel>
    {
        TEntity ToEntity(TModel model);
        TModel ToModel(TEntity entity);
        TEntity Update(TEntity entity, TModel model);
    }
}
