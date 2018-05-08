using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Converters
{
    public class ReportSettingConverter : EntityModelConverterBase<ReportSetting, ReportSettingModel>
    {
        private DataFilterConverter dataFilterConverter = new DataFilterConverter();
        private PeriodFilterConverter periodFilterConverter = new PeriodFilterConverter();

        protected override void FillEntity(ReportSetting entity, ReportSettingModel model)
        {
            entity.UserId = model.UserId;
            entity.ReportType = model.ChartType;
            entity.DataType = model.DataType;
            entity.CategoryLevel = model.CategoryLevel;

            if (entity.PeriodFilter != null)
                periodFilterConverter.UpdateEntity(entity.PeriodFilter, model.PeriodFilter);

            if (entity.DataFilter != null)
                dataFilterConverter.UpdateEntity(entity.DataFilter, model.DataFilter);
        }

        protected override void FillModel(ReportSetting entity, ReportSettingModel model)
        {
            model.UserId = entity.UserId;
            model.ChartType = entity.ReportType;
            model.DataType = entity.DataType ?? Model.Enums.RecordType.Expense;
            model.CategoryLevel = entity.CategoryLevel;

            if (entity.PeriodFilter != null)
                periodFilterConverter.UpdateModel(entity.PeriodFilter, model.PeriodFilter);

            if (entity.DataFilter != null)
                dataFilterConverter.UpdateModel(entity.DataFilter, model.DataFilter);
        }
    }
}
