using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Attributes;
using MoneyChest.Data.Entities.History;
using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(CategoryHistory))]
    public class Category : IHasId, IHasUserId
    {
        public Category()
        {
            ChildCategories = new List<Category>();
            SimpleEvents = new List<SimpleEvent>();
            Limits = new List<Limit>();
            Records = new List<Record>();
            Debts = new List<Debt>();
            MoneyTransfers = new List<MoneyTransfer>();
            ForecastSettings = new List<ForecastSetting>();
            DataFilters = new List<DataFilter>();
            ReportSettings = new List<ReportSetting>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
        
        public RecordType? RecordType { get; set; }
        
        public bool IsActive { get; set; }

        [StringLength(4000)]
        public string Remark { get; set; }

        public int? ParentCategoryId { get; set; }

        [Required]
        public int UserId { get; set; }


        #region Navigation properties

        [ForeignKey(nameof(ParentCategoryId))]
        public virtual Category ParentCategory { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

                
        public virtual ICollection<Category> ChildCategories { get; set; }
        public virtual ICollection<SimpleEvent> SimpleEvents { get; set; }
        public virtual ICollection<Limit> Limits { get; set; }
        public virtual ICollection<Record> Records { get; set; }
        public virtual ICollection<Debt> Debts { get; set; }
        public virtual ICollection<MoneyTransfer> MoneyTransfers { get; set; }
        public virtual ICollection<ForecastSetting> ForecastSettings { get; set; }
        public virtual ICollection<DataFilter> DataFilters { get; set; }
        public virtual ICollection<ReportSetting> ReportSettings { get; set; }

        #endregion
    }
}
