using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.ViewModel.Extensions
{
    public static class DataFilterExtensions
    {
        public static List<Func<ITransaction, bool>> BuildFilters(this DataFilterModel dataFilter)
        {
            var filters = new List<Func<ITransaction, bool>>();

            if (dataFilter.IsFilterApplied)
            {
                if (!string.IsNullOrEmpty(dataFilter.Description))
                    filters.Add((t) => !string.IsNullOrEmpty(t.Description) 
                        && t.Description.ToLower().Contains(dataFilter.Description.ToLower()));

                if (!string.IsNullOrEmpty(dataFilter.Remark))
                    filters.Add((t) => !string.IsNullOrEmpty(t.Remark) && t.Remark.Contains(dataFilter.Remark));

                if (dataFilter.TransactionType.HasValue)
                    filters.Add((t) => t.TransactionType == dataFilter.TransactionType.Value);

                if (dataFilter.CategoryIds.Count > 0)
                    filters.Add((t) => (dataFilter.CategoryIds.Contains(-1) && t.TransactionCategory == null)
                        || (t.TransactionCategory != null && dataFilter.CategoryIds.Contains(t.TransactionCategory.Id)));

                if (dataFilter.StorageIds.Count > 0)
                    filters.Add((t) => t.TransactionStorageIds.Any(x => dataFilter.StorageIds.Contains(x)));
            }

            return filters;
        }

        public static List<T> ApplyFilter<T>(this DataFilterModel dataFilter, IEnumerable<T> items)
            where T : ITransaction
        {
            var filters = dataFilter.BuildFilters();
            return items.Where(x => filters.Count == 0 || filters.All(f => f(x))).ToList();
        }
    }
}
