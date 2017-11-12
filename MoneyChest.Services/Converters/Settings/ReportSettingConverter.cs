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
        protected override void FillEntity(ReportSetting entity, ReportSettingModel model)
        {
            entity.UserId = model.UserId;
            entity.AllCategories = model.AllCategories;
            entity.IncludeRecordsWithoutCategory = model.IncludeRecordsWithoutCategory;
            entity.ReportType = model.ReportType;
            entity.PeriodFilterType = model.PeriodFilterType;
            entity.DataType = model?.DataType;
            entity.CategoryLevel = model.CategoryLevel;
            entity.DateFrom = model?.DateFrom;
            entity.DateUntil = model?.DateUntil;
        }

        protected override void FillModel(ReportSetting entity, ReportSettingModel model)
        {
            model.UserId = entity.UserId;
            model.AllCategories = entity.AllCategories;
            model.IncludeRecordsWithoutCategory = entity.IncludeRecordsWithoutCategory;
            model.ReportType = entity.ReportType;
            model.PeriodFilterType = entity.PeriodFilterType;
            model.DataType = entity?.DataType;
            model.CategoryLevel = entity.CategoryLevel;
            model.DateFrom = entity?.DateFrom;
            model.DateUntil = entity?.DateUntil;
            model.CategoryIds = entity.Categories.Select(e => e.Id).ToList();
        }
    }
}
