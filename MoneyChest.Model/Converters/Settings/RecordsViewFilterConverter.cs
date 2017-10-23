using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Converters
{
    public class RecordsViewFilterConverter : IEntityModelConverter<RecordsViewFilter, RecordsViewFilterModel>
    {
        public RecordsViewFilter ToEntity(RecordsViewFilterModel model)
        {
            return new RecordsViewFilter()
            {
                UserId = model.UserId,
                AllCategories = model.AllCategories,
                Description = model.Description,
                Remark = model.Remark,
                PeriodFilterType = model.PeriodFilterType,
                TransactionType = model.TransactionType,
                DateFrom = model?.DateFrom,
                DateUntil = model?.DateUntil
            };
        }

        public RecordsViewFilterModel ToModel(RecordsViewFilter entity)
        {
            return new RecordsViewFilterModel()
            {
                UserId = entity.UserId,
                AllCategories = entity.AllCategories,
                Description = entity.Description,
                Remark = entity.Remark,
                PeriodFilterType = entity.PeriodFilterType,
                TransactionType = entity.TransactionType,
                DateFrom = entity?.DateFrom,
                DateUntil = entity?.DateUntil,
                CategoryIds = entity.Categories.Select(e => e.Id).ToList()
            };
        }

        public RecordsViewFilter Update(RecordsViewFilter entity, RecordsViewFilterModel model)
        {
            entity.UserId = model.UserId;
            entity.AllCategories = model.AllCategories;
            entity.Description = model.Description;
            entity.Remark = model.Remark;
            entity.PeriodFilterType = model.PeriodFilterType;
            entity.TransactionType = model.TransactionType;
            entity.DateFrom = model?.DateFrom;
            entity.DateUntil = model?.DateUntil;

            return entity;
        }
    }
}
