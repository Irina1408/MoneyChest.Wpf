using MoneyChest.Data.Entities.Base;
using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class CategoryModel : IHasId, IHasUserId
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TransactionType? TransactionType { get; set; }

        public bool InHistory { get; set; }


        public int? ParentCategoryId { get; set; }
        public int UserId { get; set; }
    }
}
