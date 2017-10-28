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
        public CalendarDayData()
        {
            Legend = new List<LegendUnit>();
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

        public int DayOfMonth { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        
        public decimal TotalExpences { get; set; }
        public decimal TotalIncomes { get; set; }
        public decimal Balance { get; set; }
        public CurrencyReference Currency { get; set; }

        /// <summary>
        /// All expences and incomes (planned if it's day after today) in this day 
        /// </summary>
        public List<LegendUnit> Legend { get; set; }

        /// <summary>
        /// Money transfers between user accounts
        /// </summary>
        public List<MoneyTransferLegendUnit> MoneyTransferLegend { get; set; }
        
        /// <summary>
        /// All limits for this day
        /// </summary>
        public List<LimitLegendUnit> LimitLegend { get; set; }

        /// <summary>
        /// Calculated by limits recommended to spent value 
        /// </summary>
        public decimal? LimitCurrentState { get; set; }
    }
}
