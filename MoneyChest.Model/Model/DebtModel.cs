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

        public string Description { get; set; }

        public DebtType DebtType { get; set; }
        
        public DateTime TakingDate { get; set; }

        public decimal Value { get; set; }

        public decimal PaidValue { get; set; }

        public bool IsRepaid { get; set; }
        
        public DateTime? RepayingDate { get; set; }

        public DateTime? DueDate { get; set; }

        public string Remark { get; set; }
        

        public int CurrencyId { get; set; }
        public int? CategoryId { get; set; }
        public int? StorageId { get; set; }
        public int UserId { get; set; }


        public CurrencyReference Currency { get; set; }
    }
}
