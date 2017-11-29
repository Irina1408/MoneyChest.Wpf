using MoneyChest.Model.Model;
using MoneyChest.Shared.MultiLang;
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
        public static CategoryViewModelCollection BuildTree(IEnumerable<CategoryModel> categories, bool insertEmpty = false)
        {
            var result = new CategoryViewModelCollection();

            foreach (var category in categories.Where(_ => !_.ParentCategoryId.HasValue))
            {
                result.Add(BuildCategoryBranch(categories, category));
            }

            if(insertEmpty)
            {
                result.Insert(0, new CategoryViewModel()
                {
                    Id = -1,
                    Name = MultiLangResourceManager.Instance[MultiLangResourceName.None]
                });
            }

            return result;
        }

        private static CategoryViewModel BuildCategoryBranch(IEnumerable<CategoryModel> categories, CategoryModel category)
        {
            var viewModel = new CategoryViewModel()
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive,
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
