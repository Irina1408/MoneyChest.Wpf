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
        #region Expand/Collapse
        
        public static void ExpandAllMain(this CategoryViewModelCollection categories, bool isExpanded)
        {
            foreach (var category in categories.GetDescendants())
                category.IsExpandedMainView = isExpanded;
        }

        public static void ExpandAll(this CategoryViewModelCollection categories, bool isExpanded)
        {
            foreach (var category in categories.GetDescendants())
                category.IsExpanded = isExpanded;
        }

        public static void ExpandToDescendant(this CategoryViewModelCollection categories, CategoryViewModel item, bool isExpanded)
        {
            // update parent nodes
            var tmp = item;
            while (tmp.HasParent)
            {
                var parent = categories.GetDescendants().FirstOrDefault(_ => _.Id == tmp.ParentCategoryId.Value);
                parent.IsExpanded = isExpanded;
                tmp = parent;
            }
        }

        public static void ExpandMainViewToDescendant(this CategoryViewModelCollection categories, CategoryViewModel item, bool isExpanded)
        {
            // update parent nodes
            var tmp = item;
            while (tmp.HasParent)
            {
                var parent = categories.GetDescendants().FirstOrDefault(_ => _.Id == tmp.ParentCategoryId.Value);
                parent.IsExpandedMainView = isExpanded;
                tmp = parent;
            }
        }

        #endregion

        #region Select/Unselect

        public static void SelectBranch(this CategoryViewModelCollection categories, CategoryViewModel item, bool isSelected)
        {
            void SelectAll (CategoryViewModel category)
            {
                category.IsSelected = isSelected;
                foreach (var child in category.Children)
                    SelectAll(child);
            };

            SelectAll(item);
        }

        public static void SelectAll(this CategoryViewModelCollection categories, bool isSelected)
        {
            foreach (var category in categories.GetDescendants())
                category.IsSelected = isSelected;
        }

        #endregion
    }
}
