using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class CategoryModel : IHasId, IHasUserId
    {
        public CategoryModel()
        {
            InHistory = false;
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public TransactionType? TransactionType { get; set; }

        public bool InHistory { get; set; }

        [StringLength(4000)]
        public string Remark { get; set; }


        public int? ParentCategoryId { get; set; }
        public int UserId { get; set; }
    }
}
