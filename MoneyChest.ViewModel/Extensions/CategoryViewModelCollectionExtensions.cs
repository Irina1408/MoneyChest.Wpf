using MoneyChest.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.ViewModel.Extensions
{
    public static class CategoryViewModelCollectionExtensions
    {
        public static void CollapseAllMain(this CategoryViewModelCollection categories)
        {
            foreach (var category in categories.GetDescendants())
                category.IsExpandedMainView = false;
        }
        public static void ExpandAllMain(this CategoryViewModelCollection categories)
        {
            foreach (var category in categories.GetDescendants())
                category.IsExpandedMainView = true;
        }

        public static void CollapseAll(this CategoryViewModelCollection categories)
        {
            foreach (var category in categories.GetDescendants())
                category.IsExpanded = false;
        }
        public static void ExpandAll(this CategoryViewModelCollection categories)
        {
            foreach (var category in categories.GetDescendants())
                category.IsExpanded = true;
        }
    }
}
