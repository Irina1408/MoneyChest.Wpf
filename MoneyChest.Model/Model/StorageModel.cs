using MoneyChest.Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class StorageModel : IHasId, IHasUserId
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public decimal Value { get; set; }

        public string Remark { get; set; }


        public int StorageGroupId { get; set; }
        public int CurrencyId { get; set; }
        public int UserId { get; set; }

        
        public virtual CurrencyReference Currency { get; set; }
        public virtual StorageGroupReference StorageGroup { get; set; }
    }
}
