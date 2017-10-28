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
using MoneyChest.Data.Entities.Base;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(LimitHistory))]
    public class Limit : IHasId, IHasUserId
    {
        public Limit()
        {
            DateFrom = DateTime.Today.AddDays(1);
            DateUntil = DateFrom.Date;
            LimitState = LimitState.Planned;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateUntil { get; set; }

        public LimitState LimitState { get; set; }

        public decimal Value { get; set; }

        public decimal SpentValue { get; set; }
        
        public string Remark { get; set; }

        public int CurrencyId { get; set; }

        public int? CategoryId { get; set; }

        [Required]
        public int UserId { get; set; }


        [ForeignKey(nameof(CurrencyId))]
        public virtual Currency Currency { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
