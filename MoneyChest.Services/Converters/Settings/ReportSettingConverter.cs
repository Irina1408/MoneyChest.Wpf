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
            entity.IncludeActualTransactions = model.IncludeActualTransactions;
            entity.IncludeFuturePlannedTransactions = model.IncludeFuturePlannedTransactions;

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
            // check every property before update to avoid double changes handling
            if (model.UserId != entity.UserId) model.UserId = entity.UserId;
            if (model.ShowSettings != entity.ShowSettings) model.ShowSettings = entity.ShowSettings;
            if (model.ChartType != entity.ChartType) model.ChartType = entity.ChartType;
            if (model.DataType != entity.DataType) model.DataType = entity.DataType;
            if (model.CategoryLevel != entity.CategoryLevel) model.CategoryLevel = entity.CategoryLevel;
            
            if (model.Sorting != entity.Sorting) model.Sorting = entity.Sorting;
            if (model.ShowLegend != entity.ShowLegend) model.ShowLegend = entity.ShowLegend;
            if (model.ShowValue != entity.ShowValue) model.ShowValue = entity.ShowValue;
            if (model.IncludeActualTransactions != entity.IncludeActualTransactions) model.IncludeActualTransactions = entity.IncludeActualTransactions;
            if (model.IncludeFuturePlannedTransactions != entity.IncludeFuturePlannedTransactions) model.IncludeFuturePlannedTransactions = entity.IncludeFuturePlannedTransactions;

            if (model.PieChartInnerRadius != entity.PieChartInnerRadius) model.PieChartInnerRadius = entity.PieChartInnerRadius;
            if (model.PieChartDetailsDepth != entity.PieChartDetailsDepth) model.PieChartDetailsDepth = entity.PieChartDetailsDepth;
            
            if (model.BarChartView != entity.BarChartView) model.BarChartView = entity.BarChartView;
            if (model.BarChartSection != entity.BarChartSection) model.BarChartSection = entity.BarChartSection;
            if (model.BarChartSectionPeriod != entity.BarChartSectionPeriod) model.BarChartSectionPeriod = entity.BarChartSectionPeriod;
            if (model.BarChartDetail != entity.BarChartDetail) model.BarChartDetail = entity.BarChartDetail;

            if (entity.PeriodFilter != null)
                periodFilterConverter.UpdateModel(entity.PeriodFilter, model.PeriodFilter);

            if (entity.DataFilter != null)
                dataFilterConverter.UpdateModel(entity.DataFilter, model.DataFilter);
        }
    }
}
