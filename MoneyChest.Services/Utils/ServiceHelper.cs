using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using MoneyChest.Model.Base;

namespace MoneyChest.Services.Utils
{
    internal static class ServiceHelper
    {
        public static Tuple<DateTime, DateTime> GetPeriod(PeriodFilterType period, DayOfWeek firstDayOfWeek)
        {
            int tmp, year, month;

            switch (period)
            {
                case PeriodFilterType.Today:
                    return new Tuple<DateTime, DateTime>(DateTime.Today, DateTime.Today.AddDays(1));

                case PeriodFilterType.Yesterday:
                    return new Tuple<DateTime, DateTime>(DateTime.Today.AddDays(-1), DateTime.Today.AddMilliseconds(-1));

                case PeriodFilterType.ThisWeek:
                    // the past number of days this week
                    tmp = (int)DateTime.Today.DayOfWeek >= (int)firstDayOfWeek
                        ? (int)DateTime.Today.DayOfWeek - (int)firstDayOfWeek
                        : 7 - (int)firstDayOfWeek + (int)DateTime.Today.DayOfWeek;

                    return new Tuple<DateTime, DateTime>(DateTime.Today.AddDays(-tmp), DateTime.Today.AddDays(7-tmp));

                case PeriodFilterType.PreviousWeek:
                    // the past number of days this week
                    tmp = (int)DateTime.Today.DayOfWeek >= (int)firstDayOfWeek
                        ? (int)DateTime.Today.DayOfWeek - (int)firstDayOfWeek
                        : 7 - (int)firstDayOfWeek + (int)DateTime.Today.DayOfWeek;

                    return new Tuple<DateTime, DateTime>(
                        DateTime.Today.AddDays(-tmp - 7), 
                        DateTime.Today.AddDays(-tmp).AddMilliseconds(-1));

                case PeriodFilterType.ThisMonth:
                    year = DateTime.Today.Month < 12 ? DateTime.Today.Year : DateTime.Today.Year + 1;
                    month = DateTime.Today.Month < 12 ? DateTime.Today.Month + 1 : 1;

                    return new Tuple<DateTime, DateTime>(
                        new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1),
                        new DateTime(year, month, 1).AddMilliseconds(-1));

                case PeriodFilterType.PreviousMonth:
                    year = DateTime.Today.Month > 1 ? DateTime.Today.Year : DateTime.Today.Year - 1;
                    month = DateTime.Today.Month > 1 ? DateTime.Today.Month - 1 : 12;

                    return new Tuple<DateTime, DateTime>(
                        new DateTime(year, month, 1), 
                        new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMilliseconds(-1));

                case PeriodFilterType.ThisYear:
                    return new Tuple<DateTime, DateTime>(
                        new DateTime(DateTime.Today.Year, 1, 1), 
                        new DateTime(DateTime.Today.Year + 1, 1, 1).AddMilliseconds(-1));

                case PeriodFilterType.PreviousYear:
                    return new Tuple<DateTime, DateTime>(
                        new DateTime(DateTime.Today.Year - 1, 1, 1),
                        new DateTime(DateTime.Today.Year, 1, 1).AddMilliseconds(-1));

                default:
                    return null;
            }
        }

        public static void UpdateDescription<T>(ApplicationDbContext context, T entity)
            where T : IHasDescription, IHasCategory
        {
            // check this entity with empty description and populated category
            if (string.IsNullOrEmpty(entity.Description) && entity.CategoryId.HasValue)
            {
                var category = context.Categories.FirstOrDefault(x => x.Id == entity.CategoryId);
                entity.Description = category?.Name;
            }
        }

        public static void UpdateDescription<T>(ApplicationDbContext context, IEnumerable<T> entities)
            where T: IHasDescription, IHasCategory
        {
            // check there is any entity with empty description and populated category
            if (entities.Where(x => string.IsNullOrEmpty(x.Description) && x.CategoryId.HasValue).Any())
            {
                var categoryIds = entities.Where(x => x.CategoryId != null).Select(x => x.CategoryId).Distinct().ToList();
                var categories = context.Categories.Where(x => categoryIds.Contains(x.Id));

                foreach (var entity in entities.Where(x => string.IsNullOrEmpty(x.Description)).ToList())
                {
                    var category = categories.FirstOrDefault(x => x.Id == entity.CategoryId);
                    entity.Description = category?.Name;
                }
            }
        }
    }
}
