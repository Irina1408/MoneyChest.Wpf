using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using MoneyChest.Model.Enums;
using MoneyChest.Model.Model;
using MoneyChest.Model.Report;
using MoneyChest.View.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MoneyChest.View.Pages
{
    public class ChartDataBuilder
    {
        #region Private fields

        private CartesianMapper<ReportUnit> reportUnitMapperColumn;
        private CartesianMapper<ReportUnit> reportUnitMapperRow;
        private PieMapper<ReportUnit> reportUnitMapperPie;
        private ReportSettingModel settings;
        private List<ReportUnit> reportUnits;

        #endregion

        #region Initialization

        public ChartDataBuilder()
        {
            InitializeChartMappers();
        }

        private void InitializeChartMappers()
        {
            reportUnitMapperColumn = Mappers.Xy<ReportUnit>()
                .X((value, index) => index)                  // lets use the position of the item as X
                .Y(value => value.DoubleAmount);              // lets use Amount property as Y
                                                              //.Stroke((value, index) => GetBrush(value))
                                                              //.Fill((value, index) => GetBrush(value));

            reportUnitMapperRow = Mappers.Xy<ReportUnit>()
                .X(value => value.DoubleAmount)              // lets use Amount property as X
                .Y((value, index) => index);                  // lets use the position of the item as Y
                                                              //.Stroke((value, index) => GetBrush(value))
                                                              //.Fill((value, index) => GetBrush(value));

            reportUnitMapperPie = Mappers.Pie<ReportUnit>()
                .Value(value => value.DoubleAmount);              // lets use Amount property as Value
                //.Stroke((value, index) => GetBrush(value))
                //.Fill((value, index) => GetBrush(value));

            //lets save mappers globally
            //Charting.For<ReportUnit>(reportUnitMapperColumn, SeriesOrientation.Vertical);
            //Charting.For<ReportUnit>(reportUnitMapperRow, SeriesOrientation.Horizontal);
        }

        #endregion

        #region Public methods

        public ChartDataBuilder WithReportUnits(List<ReportUnit> reportUnits)
        {
            this.reportUnits = reportUnits;
            return this;
        }

        public ChartDataBuilder WithSettings(ReportSettingModel settings)
        {
            this.settings = settings;
            return this;
        }

        public ChartData Build()
        {
            var result = new ChartData();

            // save global mapper
            if (settings.IsPieChartSelected)
                Charting.For<ReportUnit>(reportUnitMapperPie);
            if (settings.IsBarChartColumnsSelected)
                Charting.For<ReportUnit>(reportUnitMapperColumn);
            if (settings.IsBarChartRowsSelected)
                Charting.For<ReportUnit>(reportUnitMapperRow);

            for (int i = 0; i < reportUnits.Count; i++)
            {
                // build collection
                if (settings.IsBarChartSelected && settings.BarChartSection == BarChartSection.Period)
                {
                    if (result.SeriesCollection.Count == 0)
                        result.SeriesCollection.AddRange(BuildSeries(reportUnits[i], i, reportUnits.Count, false));

                    if (settings.DataType == ReportDataType.All)
                    {
                        for (int iDetail = 0; iDetail < reportUnits[i].Detailing.Count; iDetail++)
                            result.SeriesCollection[iDetail].Values[i] = reportUnits[i].Detailing[iDetail];
                    }
                    else
                        result.SeriesCollection[0].Values[i] = reportUnits[i];
                }
                else
                {
                    result.SeriesCollection.AddRange(BuildSeries(reportUnits[i], i, reportUnits.Count, true));
                }

                // populate caption list
                result.Titles.Add(reportUnits[i].Caption);
            }

            // build colors collection
            var totalUnitsCount = reportUnits.Count + DetailsCount(reportUnits);
            result.ColorsCollection.AddRange(ColorGenerator.GenerateColors(totalUnitsCount));

            return result;
        }

        public void UpdateShowLables(SeriesCollection seriesCollection, bool showLables)
        {
            foreach(var series in seriesCollection)
            {
                if (series is MCPieSeries pieSeries)
                    pieSeries.DataLabels = showLables;
                if (series is MCStackedColumnSeries stackedColumnSeries)
                    stackedColumnSeries.DataLabels = showLables;
                if (series is MCStackedRowSeries stackedRowSeries)
                    stackedRowSeries.DataLabels = showLables;
                if (series is MCRowSeries rowSeries)
                    rowSeries.DataLabels = showLables;
                if (series is MCColumnSeries columnSeries)
                    columnSeries.DataLabels = showLables;
            }
        }

        #endregion

        #region Private methods

        private IEnumerable<ISeriesView> BuildSeries(ReportUnit reportUnit, int itemIndex, int totalCount, bool stacked)
        {
            var result = new List<ISeriesView>();

            // series for pie chart
            if (settings.IsPieChartSelected)
            {
                // values count should be equivalent to details depth (levels count)
                var valuesCount = settings.PieChartDetailsDepth + 1;
                // add series for current report unit
                var series = BuildSeries<MCPieSeries>(reportUnit, 0, valuesCount);
                result.Add(series);
                // add all detailing report units as new series with next details depth (level)
                result.AddRange(BuildDetailsPieSeries(reportUnit, series, 1, valuesCount));
            }

            // series for bar chart with columns
            if (settings.IsBarChartColumnsSelected && stacked)
                result.AddRange(BuildBarChartSeries<MCStackedColumnSeries>(reportUnit, itemIndex, totalCount));
            else if (settings.IsBarChartColumnsSelected && !stacked)
                result.AddRange(BuildBarChartSeries<MCColumnSeries>(reportUnit, itemIndex, totalCount));

            // series for bar chart with rows
            if (settings.IsBarChartRowsSelected && stacked)
                result.AddRange(BuildBarChartSeries<MCStackedRowSeries>(reportUnit, itemIndex, totalCount));
            else if (settings.IsBarChartRowsSelected && !stacked)
                result.AddRange(BuildBarChartSeries<MCRowSeries>(reportUnit, itemIndex, totalCount));

            return result;
        }

        private IEnumerable<ISeriesView> BuildDetailsPieSeries(ReportUnit parentReportUnit, ISeriesView parentSeries,
            int valueIndex, int valuesCount)
        {
            var result = new List<ISeriesView>();

            foreach (var reportUnit in parentReportUnit.Detailing)
            {
                ISeriesView series = null;
                // if detailing report unit category is different create new series else continue parent series
                if (reportUnit.CategoryId != parentReportUnit.CategoryId)
                {
                    series = BuildSeries<MCPieSeries>(reportUnit, valueIndex, valuesCount);
                    result.Add(series);
                }
                else
                {
                    series = parentSeries;
                    series.Values[valueIndex] = reportUnit;
                }

                // build series for every detailed report unit as new level (next value index)
                if (reportUnit.Detailing.Count > 0)
                    result.AddRange(BuildDetailsPieSeries(reportUnit, series, valueIndex + 1, valuesCount));
            }

            return result;
        }

        private List<ISeriesView> BuildBarChartSeries<TSeries>(ReportUnit reportUnit, int itemIndex, int totalCount)
            where TSeries : Series, new()
        {
            var result = new List<ISeriesView>();

            if (reportUnit.Detailing.Count > 0)
            {
                foreach (var reportUnitDetail in reportUnit.Detailing)
                    result.Add(BuildSeries<TSeries>(reportUnitDetail, itemIndex, totalCount));
            }
            else
                result.Add(BuildSeries<TSeries>(reportUnit, itemIndex, totalCount));

            return result;
        }

        private ISeriesView BuildSeries<TSeries>(ReportUnit reportUnit, int itemIndex = 0, int itemsCount = 1)
            where TSeries : Series, new()
        {
            var series = new TSeries()
            {
                Title = reportUnit.Caption,
                Values = new ChartValues<ReportUnit>(),
                DataLabels = settings.ShowValue
            };

            // all previous and next values should be equal to 0 but not current 
            for (int i = 0; i < itemsCount; i++)
                series.Values.Add(i != itemIndex ? new ReportUnit() : reportUnit);

            return series;
        }

        private Brush GetBrush(ReportUnit reportUnit)
        {
            return Brushes.Blue;
        }

        private int DetailsCount(List<ReportUnit> reportUnits)
        {
            int tot = 0;
            foreach (var reportUnit in reportUnits)
                tot += reportUnit.Detailing.Count + DetailsCount(reportUnit.Detailing);
            return tot;
        }

        #endregion
    }
}
