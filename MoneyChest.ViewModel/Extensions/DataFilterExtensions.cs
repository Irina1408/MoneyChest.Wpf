using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public static Func<T, bool> BuildFilter<T>(this DataFilterModel dataFilter)
            where T : ITransaction
        {            
            var filterExpressions = new List<Expression<Func<T, bool>>>();

            if (dataFilter.IsFilterApplied)
            {
                if (!string.IsNullOrEmpty(dataFilter.Description))
                    filterExpressions.Add((t) => !string.IsNullOrEmpty(t.Description)
                        && t.Description.ToLower().Contains(dataFilter.Description.ToLower()));

                if (!string.IsNullOrEmpty(dataFilter.Remark))
                    filterExpressions.Add((t) => !string.IsNullOrEmpty(t.Remark) && t.Remark.ToLower().Contains(dataFilter.Remark.ToLower()));

                if (dataFilter.TransactionType.HasValue)
                    filterExpressions.Add((t) => t.TransactionType == dataFilter.TransactionType.Value);

                if (dataFilter.CategoryIds.Count > 0)
                    filterExpressions.Add((t) => (dataFilter.CategoryIds.Contains(-1) && t.TransactionCategory == null)
                        || (t.TransactionCategory != null && dataFilter.CategoryIds.Contains(t.TransactionCategory.Id)));

                if (dataFilter.StorageIds.Count > 0)
                    filterExpressions.Add((t) => t.TransactionStorageIds.Any(x => dataFilter.StorageIds.Contains(x)));
            }            
            
            return (filterExpressions.Count == 0 ? (T t) => true : filterExpressions.Combine()).Compile();
        }

        public static List<T> ApplyFilter<T>(this DataFilterModel dataFilter, IEnumerable<T> items)
            where T : ITransaction
        {
            //var filters = dataFilter.BuildFilters();
            //var filterExpression = dataFilter.BuildFilter<T>();
            //return items.AsQueryable().Where(filterExpression).ToList();
            return !dataFilter.IsFilterApplied
                ? items.ToList()
                : items.Where(t =>
                    (string.IsNullOrEmpty(dataFilter.Description) || (!string.IsNullOrEmpty(t.Description)
                        && t.Description.ToLower().Contains(dataFilter.Description.ToLower())))

                    && (string.IsNullOrEmpty(dataFilter.Remark) || (!string.IsNullOrEmpty(t.Remark) && t.Remark.ToLower().Contains(dataFilter.Remark.ToLower())))

                    && (dataFilter.TransactionType == null || t.TransactionType == dataFilter.TransactionType.Value)

                    && (dataFilter.CategoryIds.Count == 0 || ((dataFilter.CategoryIds.Contains(-1) && t.TransactionCategory == null)
                        || (t.TransactionCategory != null && dataFilter.CategoryIds.Contains(t.TransactionCategory.Id))))

                    && (dataFilter.StorageIds.Count == 0 || t.TransactionStorageIds.Any(x => dataFilter.StorageIds.Contains(x))))
                    .ToList();
        }
        
        private static Expression<Func<T, bool>> Combine<T>(this List<Expression<Func<T, bool>>> expressions)
            where T : ITransaction
        {
            Expression<Func<T, bool>> expression = null;

            foreach(var expr in expressions)
            {
                if (expression == null)
                    expression = expr;
                else
                {
                    var body = Expression<Func<T, bool>>.AndAlso(expression.Body, expr.Body);
                    expression = Expression.Lambda<Func<T, bool>>(body, expression.Parameters[0]);
                }
            }

            return expression;
        }
    }
}
