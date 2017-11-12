using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Converters
{
    public interface IEntityModelConverter<TEntity, TModel>
    {
        TEntity ToEntity(TModel model);
        TModel ToModel(TEntity entity);
        TEntity UpdateEntity(TEntity entity, TModel model);
        TModel UpdateModel(TEntity entity, TModel model);
    }
}
