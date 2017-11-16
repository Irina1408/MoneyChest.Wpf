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
            var result = new ObservableCollection<MultiLangEnumDescription>();

            foreach (var enumItem in Enum.GetValues(enumType))
                result.Add(new MultiLangEnumDescription(enumType.Name, enumItem, Enum.GetName(enumType, enumItem)));

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
        public string Description => MultiLangResourceManager.Instance[$"{TypeName}_{Name}"] ?? Name;

        public void NotifyCultureChanged() =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
    }
}
