using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Attributes;
using MoneyChest.Data.Entities.History;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(StorageHistory))]
    public class Storage
    {
        public Storage()
        {
            StoreType = StoreType.Wallet;

            StorageFromMoneyTransferEvents = new List<MoneyTransferEvent>();
            StorageToMoneyTransferEvents = new List<MoneyTransferEvent>();
            StorageFromMoneyTransfers = new List<MoneyTransfer>();
            StorageToMoneyTransfers = new List<MoneyTransfer>();
            Debts = new List<Debt>();
            RepayDebtEvents = new List<RepayDebtEvent>();
            SimpleEvents = new List<SimpleEvent>();
            Records = new List<Record>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public StoreType StoreType { get; set; }

        public decimal Value { get; set; }

        public string Remark { get; set; }

        public int CurrencyId { get; set; }

        [Required]
        public int UserId { get; set; }


        #region Naviagtion properties

        [ForeignKey(nameof(CurrencyId))]
        public virtual Currency Currency { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }


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

        #endregion
    }
}
