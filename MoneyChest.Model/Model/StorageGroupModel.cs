using MoneyChest.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class StorageGroupModel : IHasId, IHasUserId
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public int UserId { get; set; }
    }
}
