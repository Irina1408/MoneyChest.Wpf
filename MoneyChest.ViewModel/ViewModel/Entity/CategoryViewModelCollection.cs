using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.ViewModel.ViewModel
{
    public class CategoryViewModelCollection : ObservableCollection<CategoryViewModel>
    {
        public CategoryViewModelCollection() : base()
        { }

        public CategoryViewModelCollection(List<CategoryViewModel> list) : base(list)
        { }

        public CategoryViewModelCollection(IEnumerable<CategoryViewModel> collection) : base(collection)
        { }

        public IEnumerable<CategoryViewModel> GetDescendants()
        {
            foreach(var item in this)
            {
                yield return item;

                foreach (var childItem in item.Children.GetDescendants())
                    yield return childItem;
            }
        }

        public bool RemoveDescendant(CategoryViewModel item)
        {
            foreach(var cat in this)
            {
                if (cat.Equals(item))
                {
                    this.Remove(item);
                    return true;
                }
                else if(cat.Children.RemoveDescendant(item))
                    return true;
            }

            return false;
        }

        public void ExpandToDescendant(CategoryViewModel item, bool isExpanded)
        {
            // update parent nodes
            var tmp = item;
            while (tmp.HasParent)
            {
                var parent = this.GetDescendants().FirstOrDefault(_ => _.Id == tmp.ParentCategoryId.Value);
                parent.IsExpanded = isExpanded;
                tmp = parent;
            }
        }

        public void ExpandMainViewToDescendant(CategoryViewModel item, bool isExpanded)
        {
            // update parent nodes
            var tmp = item;
            while (tmp.HasParent)
            {
                var parent = this.GetDescendants().FirstOrDefault(_ => _.Id == tmp.ParentCategoryId.Value);
                parent.IsExpandedMainView = isExpanded;
                tmp = parent;
            }
        }
    }
}
