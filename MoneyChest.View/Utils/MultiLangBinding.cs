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
        public MultiLangBinding(string path)
            : base($"[{path}]")
        {
            Mode = BindingMode.OneWay;
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            Source = MultiLangResourceManager.Instance;
        }
    }
}
