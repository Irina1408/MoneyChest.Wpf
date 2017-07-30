using MoneyChest.Data.Enums;
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

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(CategoryHistory))]
    public class Category
    {
        public Category()
        {
            InHistory = false;

            GeneralSettingsAsDebtCategory = new List<GeneralSetting>();
            GeneralSettingsAsComission = new List<GeneralSetting>();
            ChildCategories = new List<Category>();
            SimpleEvents = new List<SimpleEvent>();
            Limits = new List<Limit>();
            Records = new List<Record>();
            UserSettingsCategorized = new List<SettingCategorized>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public TransactionType? TransactionType { get; set; }
        
        public bool InHistory { get; set; }

        public int? ParentCategoryId { get; set; }

        [Required]
        public int UserId { get; set; }


        #region Navigation properties

        [ForeignKey(nameof(ParentCategoryId))]
        public virtual Category ParentCategory { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }


        [InverseProperty(nameof(GeneralSetting.DebtCategory))]
        public virtual ICollection<GeneralSetting> GeneralSettingsAsDebtCategory { get; set; }

        [InverseProperty(nameof(GeneralSetting.ComissionCategory))]
        public virtual ICollection<GeneralSetting> GeneralSettingsAsComission { get; set; }
        
        public virtual ICollection<Category> ChildCategories { get; set; }
        public virtual ICollection<SimpleEvent> SimpleEvents { get; set; }
        public virtual ICollection<Limit> Limits { get; set; }
        public virtual ICollection<Record> Records { get; set; }
        public virtual ICollection<SettingCategorized> UserSettingsCategorized { get; set; }

        #endregion
    }
}
