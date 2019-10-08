using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Converters
{
    public class TransactionsSettingsConverter : EntityModelConverterBase<TransactionsSettings, TransactionsSettingsModel>
    {
        private DataFilterConverter dataFilterConverter = new DataFilterConverter();
        private PeriodFilterConverter periodFilterConverter = new PeriodFilterConverter();

        protected override void FillEntity(TransactionsSettings entity, TransactionsSettingsModel model)
        {
            entity.UserId = model.UserId;
            entity.ShowTemplates = model.ShowTemplates;

            if (entity.PeriodFilter != null)
                periodFilterConverter.UpdateEntity(entity.PeriodFilter, model.PeriodFilter);

            if(entity.DataFilter != null)
                dataFilterConverter.UpdateEntity(entity.DataFilter, model.DataFilter);
        }

        protected override void FillModel(TransactionsSettings entity, TransactionsSettingsModel model)
        {
            model.UserId = entity.UserId;
            model.ShowTemplates = entity.ShowTemplates;

            if (entity.PeriodFilter != null)
                periodFilterConverter.UpdateModel(entity.PeriodFilter, model.PeriodFilter);

            if (entity.DataFilter != null)
                dataFilterConverter.UpdateModel(entity.DataFilter, model.DataFilter);
        }
    }
}
