using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Shared.MultiLang
{
    public static class MultiLangResource
    {
        #region Deletion messages

        public static string DeletionConfirmationMessage(Type entityType, IEnumerable<string> items) =>
            DeletionConfirmationMessage(entityType.Name.Replace("Model", ""), items);

        public static string DeletionConfirmationMessage(string entityName, IEnumerable<string> items) =>
            string.Format(MultiLangResourceManager.Instance[MultiLangResourceName.DeletionConfirmationMessage],
                items == null || items.Count() <= 1 
                ? MultiLangResourceManager.Instance[MultiLangResourceName.Singular(entityName)]?.ToLower()
                : MultiLangResourceManager.Instance[MultiLangResourceName.Plural(entityName)]?.ToLower(),
                string.Join(";", items));

        public static string DeletionErrorMessage(Type entityType, IEnumerable<string> items) =>
            DeletionErrorMessage(entityType.Name.Replace("Model", ""), items);

        public static string DeletionErrorMessage(string entityName, IEnumerable<string> items) =>
            items == null || items.Count() <= 1
            ? string.Format(MultiLangResourceManager.Instance[MultiLangResourceName.DeletionErrorMessageOne],
                MultiLangResourceManager.Instance[MultiLangResourceName.Singular(entityName)], string.Join(";", items))
            : string.Format(MultiLangResourceManager.Instance[MultiLangResourceName.DeletionErrorMessagePlural],
                MultiLangResourceManager.Instance[MultiLangResourceName.Plural(entityName)], string.Join(";", items));

        #endregion

        #region Enum

        public static string EnumItemDescription(string typeName, string name) => 
            MultiLangResourceManager.Instance[$"{typeName}_{name}"] ?? name;

        public static string EnumItemDescription(Type enumType, object value) => 
            EnumItemDescription(enumType.Name, Enum.GetName(enumType, value));

        #endregion

        #region Schedule

        public static string EveryNumberDays(int number) => 
            string.Format(MultiLangResourceManager.Instance[MultiLangResourceName.EveryNumberDays], number);

        public static string EveryWeek(ICollection<DayOfWeek> daysOfWeek) =>
            string.Format(MultiLangResourceManager.Instance[MultiLangResourceName.EveryWeek], GetWeeks(daysOfWeek));

        public static string EveryWeek(ICollection<DayOfWeek> daysOfWeek, int weekNumber) =>
            string.Format(MultiLangResourceManager.Instance[MultiLangResourceName.EveryNumberWeeks], GetWeeks(daysOfWeek), weekNumber);

        public static string EveryWeek(int weekNumber) =>
            string.Format(MultiLangResourceManager.Instance[MultiLangResourceName.EveryDayNumberWeeks], weekNumber);

        public static string EveryMonth(ICollection<Month> months, int dayOfMonth) =>
            string.Format(MultiLangResourceManager.Instance[MultiLangResourceName.EverySpecialMonth], GetMonthes(months), dayOfMonth);

        public static string EveryMonth(int dayOfMonth) =>
            string.Format(MultiLangResourceManager.Instance[MultiLangResourceName.EveryMonth], dayOfMonth);

        public static string EveryMonthLastDay(ICollection<Month> months) =>
            string.Format(MultiLangResourceManager.Instance[MultiLangResourceName.EverySpecialMonthLastDay], GetMonthes(months));

        public static string EveryMonthLastDay() =>
            string.Format(MultiLangResourceManager.Instance[MultiLangResourceName.EveryMonthLastDay]);

        public static string PausedToDate(DateTime date) =>
            string.Format(MultiLangResourceManager.Instance[MultiLangResourceName.PausedToDate], date.ToShortDateString());

        private static string GetWeeks(ICollection<DayOfWeek> daysOfWeek)
        {
            var sb = new StringBuilder();

            // get description for every week
            foreach (var dayOfWeek in daysOfWeek)
            {
                if (sb.Length > 0) sb.Append(", ");
                sb.Append(MultiLangResourceManager.Instance[MultiLangResourceName.InDayOfWeek(dayOfWeek)]);
            }

            return sb.ToString();
        }

        private static string GetMonthes(ICollection<Month> months)
        {
            var sb = new StringBuilder();

            // get description for every month
            foreach (var month in months)
            {
                if (sb.Length > 0) sb.Append(", ");
                sb.Append(EnumItemDescription(nameof(Month), month.ToString()));
            }

            return sb.ToString();
        }

        #endregion
    }
}
