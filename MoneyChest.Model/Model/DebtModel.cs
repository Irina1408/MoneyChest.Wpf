using MoneyChest.Data.Entities.Base;
using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class DebtModel : IHasId, IHasUserId
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DebtType DebtType { get; set; }
        
        public DateTime TakingDate { get; set; }

        public decimal Value { get; set; }

        public decimal PaidValue { get; set; }

        public bool IsRepayed { get; set; }
        
        public DateTime? RepayingDate { get; set; }

        public string Remark { get; set; }
        

        public int CurrencyId { get; set; }
        public int? StorageId { get; set; }
        public int UserId { get; set; }


        public CurrencyReference Currency { get; set; }
    }
}
