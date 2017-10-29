using MoneyChest.Data.Attributes;
using MoneyChest.Data.Entities.Base;
using MoneyChest.Data.Entities.History;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(CurrencyHistory))]
    public class Currency : IHasId, IHasUserId
    {
        public Currency()
        {
            IsUsed = true;
            SymbolAlignmentIsRight = true;

            CurrencyExchangeRateFroms = new List<CurrencyExchangeRate>();
            CurrencyExchangeRateTos = new List<CurrencyExchangeRate>();
            Debts = new List<Debt>();
            SimpleEvents = new List<SimpleEvent>();
            Limits = new List<Limit>();
            Records = new List<Record>();
            Storages = new List<Storage>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(10)]
        public string Symbol { get; set; }
        
        public bool IsUsed { get; set; }
        
        public bool IsMain { get; set; }

        public bool SymbolAlignmentIsRight { get; set; }

        [Required]
        public int UserId { get; set; }
        
        #region Navigation properties
        
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [InverseProperty(nameof(CurrencyExchangeRate.CurrencyFrom))]
        public virtual ICollection<CurrencyExchangeRate> CurrencyExchangeRateFroms { get; set; }

        [InverseProperty(nameof(CurrencyExchangeRate.CurrencyTo))]
        public virtual ICollection<CurrencyExchangeRate> CurrencyExchangeRateTos { get; set; }

        public virtual ICollection<Debt> Debts { get; set; }
        public virtual ICollection<SimpleEvent> SimpleEvents { get; set; }
        public virtual ICollection<Limit> Limits { get; set; }
        public virtual ICollection<Record> Records { get; set; }
        public virtual ICollection<Storage> Storages { get; set; }

        #endregion
    }
}
