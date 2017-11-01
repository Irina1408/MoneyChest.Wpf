using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Model.Base;

namespace MoneyChest.Model.Model
{
    public class RecordModel : IHasId, IHasUserId
    {
        public RecordModel()
        {
            Date = DateTime.Now;
            TransactionType = TransactionType.Expense;
        }
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public TransactionType TransactionType { get; set; }

        public decimal Value { get; set; }

        public string Remark { get; set; }


        public int? CategoryId { get; set; }
        public int CurrencyId { get; set; }
        public int? StorageId { get; set; }
        public int? DebtId { get; set; }
        public int UserId { get; set; }

        
        public CategoryReference Category { get; set; }
        public CurrencyReference Currency { get; set; }
        public StorageReference Storage { get; set; }
        public DebtReference Debt { get; set; }
    }
}
