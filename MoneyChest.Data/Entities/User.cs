using MoneyChest.Data.Entities.History;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Attributes;
using MoneyChest.Model.Base;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(UserHistory))]
    public class User : IHasId
    {
        public User()
        {
            FirstUsageDate = DateTime.Now;

            Categories = new List<Category>();
            Currencies = new List<Currency>();
            Debts = new List<Debt>();
            Events = new List<Evnt>();
            Limits = new List<Limit>();
            Records = new List<Record>();
            Storages = new List<Storage>();
            StorageGroups = new List<StorageGroup>();

            CategoriesHistory = new List<CategoryHistory>();
            CurrenciesHistory = new List<CurrencyHistory>();
            DebtsHistory = new List<DebtHistory>();
            DebtPenaltiesHistory = new List<DebtPenaltyHistory>();
            EventsHistory = new List<EventHistory>();
            LimitsHistory = new List<LimitHistory>();
            RecordsHistory = new List<RecordHistory>();
            StoragesHistory = new List<StorageHistory>();
            StorageGroupsHistory = new List<StorageGroupHistory>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        public DateTime FirstUsageDate { get; set; }

        public DateTime LastUsageDate { get; set; }

        public DateTime? LastSynchronizationDate { get; set; }

        public string ServerUserId { get; set; }

        #region Navigation properties
        
        public virtual CalendarSetting CalendarSettings { get; set; }
        public virtual ForecastSetting ForecastSettings { get; set; }
        public virtual GeneralSetting GeneralSettings { get; set; }
        public virtual RecordsViewFilter RecordsViewFilter { get; set; }
        public virtual ReportSetting ReportSettings { get; set; }


        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Currency> Currencies { get; set; }
        public virtual ICollection<Debt> Debts { get; set; }
        public virtual ICollection<Evnt> Events { get; set; }
        public virtual ICollection<Limit> Limits { get; set; }
        public virtual ICollection<Record> Records { get; set; }
        public virtual ICollection<Storage> Storages { get; set; }
        public virtual ICollection<StorageGroup> StorageGroups { get; set; }
        public virtual ICollection<CategoryHistory> CategoriesHistory { get; set; }
        public virtual ICollection<CurrencyHistory> CurrenciesHistory { get; set; }
        public virtual ICollection<DebtHistory> DebtsHistory { get; set; }
        public virtual ICollection<DebtPenaltyHistory> DebtPenaltiesHistory { get; set; }
        public virtual ICollection<EventHistory> EventsHistory { get; set; }
        public virtual ICollection<LimitHistory> LimitsHistory { get; set; }
        public virtual ICollection<RecordHistory> RecordsHistory { get; set; }
        public virtual ICollection<StorageHistory> StoragesHistory { get; set; }
        public virtual ICollection<StorageGroupHistory> StorageGroupsHistory { get; set; }

        #endregion
    }
}
