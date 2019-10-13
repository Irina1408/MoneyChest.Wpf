using MoneyChest.Shared.MultiLang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MoneyChest.View
{
    public class MultiLangBinding : Binding
    {
        public MultiLangBinding() : base()
        {
            Mode = BindingMode.OneWay;
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            Source = MultiLangResourceManager.Instance;
        }

        public MultiLangBinding(string path) : this()
            //: base($"[{path}]")
        {
            //Mode = BindingMode.OneWay;
            //UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //Source = MultiLangResourceManager.Instance;
            MultiLangPath = path;
        }

        public string MultiLangPath
        {
            set => Path = new System.Windows.PropertyPath($"[{value}]");
        }
    }

    //public class MultiLangEnumBinding : MultiLangBinding
    //{
    //    public MultiLangEnumBinding(object enumItem)
    //        : base($"[{MultiLangResourceName.EnumValue(enumItem.GetType(), Enum.GetName(enumItem.GetType(), enumItem))}]")
    //    { }
    //}
}
