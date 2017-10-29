using MoneyChest.Data.Entities.Base;
using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class LimitModel : IHasId, IHasUserId
    {
        public LimitModel()
        {
            DateFrom = DateTime.Today.AddDays(1);
            DateUntil = DateFrom.Date;
            LimitState = LimitState.Planned;
        }

        public int Id { get; set; }
        
        public DateTime DateFrom { get; set; }
        
        public DateTime DateUntil { get; set; }

        public LimitState LimitState { get; set; }

        public decimal Value { get; set; }

        public decimal SpentValue { get; set; }

        public string Remark { get; set; }


        public int CurrencyId { get; set; }
        public int? CategoryId { get; set; }
        public int UserId { get; set; }


        public CurrencyReference Currency { get; set; }
        public CategoryReference Category { get; set; }
    }
}
