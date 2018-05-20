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
            entity.ShowSettings = model.ShowSettings;
            entity.ChartType = model.ChartType;
            entity.DataType = model.DataType;
            entity.CategoryLevel = model.CategoryLevel;

            entity.Sorting = model.Sorting;
            entity.ShowLegend = model.ShowLegend;
            entity.ShowValue = model.ShowValue;

            entity.PieChartInnerRadius = model.PieChartInnerRadius;
            entity.PieChartDetailsDepth = model.PieChartDetailsDepth;

            entity.BarChartView = model.BarChartView;
            entity.BarChartSection = model.BarChartSection;
            entity.BarChartSectionPeriod = model.BarChartSectionPeriod;
            entity.BarChartDetail = model.BarChartDetail;

            if (entity.PeriodFilter != null)
                periodFilterConverter.UpdateEntity(entity.PeriodFilter, model.PeriodFilter);

            if (entity.DataFilter != null)
                dataFilterConverter.UpdateEntity(entity.DataFilter, model.DataFilter);
        }

        protected override void FillModel(ReportSetting entity, ReportSettingModel model)
        {
            model.UserId = entity.UserId;
            model.ShowSettings = entity.ShowSettings;
            model.ChartType = entity.ChartType;
            model.DataType = entity.DataType;
            model.CategoryLevel = entity.CategoryLevel;
            
            model.Sorting = entity.Sorting;
            model.ShowLegend = entity.ShowLegend;
            model.ShowValue = entity.ShowValue;
            
            model.PieChartInnerRadius = entity.PieChartInnerRadius;
            model.PieChartDetailsDepth = entity.PieChartDetailsDepth;
            
            model.BarChartView = entity.BarChartView;
            model.BarChartSection = entity.BarChartSection;
            model.BarChartSectionPeriod = entity.BarChartSectionPeriod;
            model.BarChartDetail = entity.BarChartDetail;

            if (entity.PeriodFilter != null)
                periodFilterConverter.UpdateModel(entity.PeriodFilter, model.PeriodFilter);

            if (entity.DataFilter != null)
                dataFilterConverter.UpdateModel(entity.DataFilter, model.DataFilter);
        }
    }
}
