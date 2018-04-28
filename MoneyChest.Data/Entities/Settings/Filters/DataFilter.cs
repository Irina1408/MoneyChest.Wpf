using MoneyChest.Model.Base;
using MoneyChest.Model.Constants;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities
{
    public class DataFilter : IHasId
    {
        public DataFilter()
        {
            Categories = new List<Category>();
            Storages = new List<Storage>();
            TransactionsSettings = new List<TransactionsSettings>();
        }

        [Key]
        public int Id { get; set; }
        public bool IsFilterApplied { get; set; }
        public bool IsFilterVisible { get; set; } = false;
        public bool IsCategoryBranchSelection { get; set; } = false;
        public bool IncludeWithoutCategory { get; set; }
        public TransactionType? TransactionType { get; set; }

        [StringLength(MaxSize.DescriptionLength)]
        public string Description { get; set; }

        [StringLength(MaxSize.RemarkLength)]
        public string Remark { get; set; }
        
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Storage> Storages { get; set; }
        public virtual ICollection<TransactionsSettings> TransactionsSettings { get; set; }
    }
}
