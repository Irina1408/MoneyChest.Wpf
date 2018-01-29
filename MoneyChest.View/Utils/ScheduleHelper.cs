using MoneyChest.Shared.MultiLang;
using MoneyChest.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.View.Utils
{
    public class ScheduleHelper
    {
        public static List<ItemViewModel> GetMonthes()
        {
            var result = new List<ItemViewModel>();

            // add all possible numeric days of month
            for (int i = 1; i <= 31; i++)
                result.Add(new ItemViewModel(i, i.ToString()));

            // add last day of mont
            result.Add(new ItemViewModel(-1, MultiLangResourceManager.Instance[MultiLangResourceName.LastForDayOfMonth]));

            return result;
        }
    }
}
