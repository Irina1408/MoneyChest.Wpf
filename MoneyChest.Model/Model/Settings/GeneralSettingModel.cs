using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;
using System.Globalization;
using System.ComponentModel;

namespace MoneyChest.Model.Model
{
    public class GeneralSettingModel : IHasUserId, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public GeneralSettingModel()
        {
            Language = Language.English;
            FirstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
        }

        public int UserId { get; set; }
        public Language Language { get; set; }
        public DayOfWeek FirstDayOfWeek { get; set; }

        public string AccentColor { get; set; }
        public string ThemeColor { get; set; }
    }
}
