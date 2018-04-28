using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Attributes;
using MoneyChest.Data.Entities.History;
using MoneyChest.Model.Base;
using MoneyChest.Model.Constants;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(StorageHistory))]
    public class Storage : IHasId, IHasUserId
    {
        public Storage()
        {
            StorageFromMoneyTransferEvents = new List<MoneyTransferEvent>();
            StorageToMoneyTransferEvents = new List<MoneyTransferEvent>();
            StorageFromMoneyTransfers = new List<MoneyTransfer>();
            StorageToMoneyTransfers = new List<MoneyTransfer>();
            Debts = new List<Debt>();
            RepayDebtEvents = new List<RepayDebtEvent>();
            SimpleEvents = new List<SimpleEvent>();
            Records = new List<Record>();
            DataFilters = new List<DataFilter>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(MaxSize.NameLength)]
        public string Name { get; set; }

        public decimal Value { get; set; }

        public bool IsVisible { get; set; }

        [StringLength(MaxSize.RemarkLength)]
        public string Remark { get; set; }

        public int CurrencyId { get; set; }

        public int StorageGroupId { get; set; }

        [Required]
        public int UserId { get; set; }


        #region Navigation properties

        [ForeignKey(nameof(CurrencyId))]
        public virtual Currency Currency { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [ForeignKey(nameof(StorageGroupId))]
        public virtual StorageGroup StorageGroup { get; set; }


        [InverseProperty(nameof(MoneyTransferEvent.StorageFrom))]
        public virtual ICollection<MoneyTransferEvent> StorageFromMoneyTransferEvents { get; set; }

        [InverseProperty(nameof(MoneyTransferEvent.StorageTo))]
        public virtual ICollection<MoneyTransferEvent> StorageToMoneyTransferEvents { get; set; }

        [InverseProperty(nameof(MoneyTransfer.StorageFrom))]
        public virtual ICollection<MoneyTransfer> StorageFromMoneyTransfers { get; set; }

        [InverseProperty(nameof(MoneyTransfer.StorageTo))]
        public virtual ICollection<MoneyTransfer> StorageToMoneyTransfers { get; set; }

        public virtual ICollection<Debt> Debts { get; set; }
        public virtual ICollection<RepayDebtEvent> RepayDebtEvents { get; set; }
        public virtual ICollection<SimpleEvent> SimpleEvents { get; set; }
        public virtual ICollection<Record> Records { get; set; }
        public virtual ICollection<DataFilter> DataFilters { get; set; }

        #endregion
    }
}
