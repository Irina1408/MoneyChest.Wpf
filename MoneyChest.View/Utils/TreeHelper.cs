using MoneyChest.Model.Model;
using MoneyChest.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.View.Utils
{
    public static class TreeHelper
    {
        public static CategoryViewModelCollection BuildTree(IEnumerable<CategoryModel> categories)
        {
            var result = new CategoryViewModelCollection();

            foreach (var category in categories.Where(_ => !_.ParentCategoryId.HasValue))
            {
                result.Add(BuildCategoryBranch(categories, category));
            }

            return result;
        }

        private static CategoryViewModel BuildCategoryBranch(IEnumerable<CategoryModel> categories, CategoryModel category)
        {
            var viewModel = new CategoryViewModel()
            {
                Id = category.Id,
                Name = category.Name,
                InHistory = category.InHistory,
                TransactionType = category.TransactionType,
                ParentCategoryId = category.ParentCategoryId,
                Remark = category.Remark,
                UserId = category.UserId
            };

            foreach (var childCat in categories.Where(_ => _.ParentCategoryId.HasValue && _.ParentCategoryId.Value == category.Id))
            {
                viewModel.Children.Add(BuildCategoryBranch(categories, childCat));
            }

            return viewModel;
        }
    }
}
