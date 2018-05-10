using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Shared.MultiLang
{
    public static class MultiLangEnumHelper
    {
        public static ObservableCollection<MultiLangEnumDescription> ToCollection(Type enumType)
        {
            var values = new List<object>();
            foreach (var enumItem in Enum.GetValues(enumType))
                values.Add(enumItem);

            return ToCollection(enumType, values);
        }            

        public static ObservableCollection<MultiLangEnumDescription> ToCollection(Type enumType, IEnumerable<object> values)
        {
            var result = new ObservableCollection<MultiLangEnumDescription>();

            foreach (var enumItem in values)
                result.Add(new MultiLangEnumDescription(enumType.Name, enumItem, Enum.GetName(enumType, enumItem)));

            MultiLangResourceManager.Instance.CultureChanged += (sender, e) =>
            {
                foreach (var item in result)
                    item.NotifyCultureChanged();
            };

            return result;
        }

        public static ObservableCollection<SelectableMultiLangEnumDescription> ToSelectableCollection<T>(
            ICollection<T> selectedItems = null)
        {
            var enumType = typeof(T);
            var result = new ObservableCollection<SelectableMultiLangEnumDescription>();

            foreach (T enumItem in Enum.GetValues(enumType))
            {
                result.Add(new SelectableMultiLangEnumDescription(enumType.Name, enumItem, Enum.GetName(enumType, enumItem))
                {
                    IsSelected = selectedItems?.Contains(enumItem) ?? false
                });
            }

            MultiLangResourceManager.Instance.CultureChanged += (sender, e) =>
            {
                foreach (var item in result)
                    item.NotifyCultureChanged();
            };

            return result;
        }
    }

    public class MultiLangEnumDescription : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MultiLangEnumDescription(string typeName, object value, string name)
        {
            TypeName = typeName;
            Value = value;
            Name = name;
        }

        public string TypeName { get; private set; }
        public string Name { get; private set; }
        public object Value { get; private set; }
        public string Description => MultiLangResource.EnumItemDescription(TypeName, Name);

        public void NotifyCultureChanged() => NotifyPropertyChanged(nameof(Description));

        protected void NotifyPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class SelectableMultiLangEnumDescription : MultiLangEnumDescription
    {
        private bool isSelected;

        public SelectableMultiLangEnumDescription(string typeName, object value, string name) : base(typeName, value, name)
        {
        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                NotifyPropertyChanged(nameof(IsSelected));
            }
        }
    }
}
