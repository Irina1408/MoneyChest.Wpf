using MoneyChest.Calculation.Common;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Builders.Calendar
{
    public class CalendarDayData
    {
        #region Initialization

        public CalendarDayData()
        {
            RecordLegend = new List<RecordLegendUnit>();
            MoneyTransferLegend = new List<MoneyTransferLegendUnit>();
            LimitLegend = new List<LimitLegendUnit>();
        }

        public CalendarDayData(int dayOfMonth, int month, int year)
            :base()
        {
            DayOfMonth = dayOfMonth;
            Month = month;
            Year = year;
        }

        #endregion

        #region Required part

        public int DayOfMonth { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        #endregion

        #region Full legends

        public List<RecordLegendUnit> RecordLegend { get; set; }
        public List<MoneyTransferLegendUnit> MoneyTransferLegend { get; set; }
        public List<LimitLegendUnit> LimitLegend { get; set; }

        #endregion

        #region Totals

        #endregion

        // TODO: remove bottom properties
        //public decimal TotalExpences { get; set; }
        //public decimal TotalIncomes { get; set; }
        //public decimal Balance { get; set; }
        //public CurrencyReference Currency { get; set; }
        
        //public List<LegendUnit> RecordLegend { get; set; }

        /// <summary>
        /// Calculated by limits recommended to spent value 
        /// </summary>
        //public decimal? LimitCurrentState { get; set; }
    }
}
