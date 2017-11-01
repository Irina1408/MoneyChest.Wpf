using MoneyChest.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class ForecastSettingModel : IHasUserId
    {
        public ForecastSettingModel()
        {
            AllCategories = true;
            RepeatsCount = 5;
            ActualDays = 100;

            CategoryIds = new List<int>();
        }

        public int UserId { get; set; }

        public bool AllCategories { get; set; }
        
        public uint RepeatsCount { get; set; }

        public uint ActualDays { get; set; }
        
        public List<int> CategoryIds { get; set; }
    }
}
