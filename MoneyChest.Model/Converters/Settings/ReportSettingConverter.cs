using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Converters
{
    public class ReportSettingConverter : IEntityModelConverter<ReportSetting, ReportSettingModel>
    {
        public ReportSetting ToEntity(ReportSettingModel model)
        {
            return new ReportSetting()
            {
                UserId = model.UserId,
                AllCategories = model.AllCategories,
                IncludeRecordsWithoutCategory = model.IncludeRecordsWithoutCategory,
                ReportType = model.ReportType,
                PeriodFilterType = model.PeriodFilterType,
                DataType = model.DataType,
                CategoryLevel = model.CategoryLevel,
                DateFrom = model?.DateFrom,
                DateUntil = model?.DateUntil
            };
        }

        public ReportSettingModel ToModel(ReportSetting entity)
        {
            return new ReportSettingModel()
            {
                UserId = entity.UserId,
                AllCategories = entity.AllCategories,
                IncludeRecordsWithoutCategory = entity.IncludeRecordsWithoutCategory,
                ReportType = entity.ReportType,
                PeriodFilterType = entity.PeriodFilterType,
                DataType = entity.DataType,
                CategoryLevel = entity.CategoryLevel,
                DateFrom = entity?.DateFrom,
                DateUntil = entity?.DateUntil,
                CategoryIds = entity.Categories.Select(e => e.Id).ToList()
            };
        }

        public ReportSetting Update(ReportSetting entity, ReportSettingModel model)
        {
            entity.UserId = model.UserId;
            entity.AllCategories = model.AllCategories;
            entity.IncludeRecordsWithoutCategory = model.IncludeRecordsWithoutCategory;
            entity.ReportType = model.ReportType;
            entity.PeriodFilterType = model.PeriodFilterType;
            entity.DataType = model.DataType;
            entity.CategoryLevel = model.CategoryLevel;
            entity.DateFrom = model?.DateFrom;
            entity.DateUntil = model?.DateUntil;

            return entity;
        }
    }
}
