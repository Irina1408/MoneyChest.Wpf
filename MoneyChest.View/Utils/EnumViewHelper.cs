using MahApps.Metro.IconPacks;
using MoneyChest.Model.Enums;
using MoneyChest.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.View.Utils
{
    public static class EnumViewHelper
    {
        public static List<EnumIcon> GetRecordTypeIcons()
        {
            return new List<EnumIcon>()
            {
                new EnumIcon()
                {
                    Value = RecordType.Expense,
                    Icon = new PackIconMaterial() { Kind = PackIconMaterialKind.Minus }
                },
                new EnumIcon()
                {
                    Value = RecordType.Income,
                    Icon = new PackIconMaterial() { Kind = PackIconMaterialKind.Plus }
                }
            };
        }
    }
}
