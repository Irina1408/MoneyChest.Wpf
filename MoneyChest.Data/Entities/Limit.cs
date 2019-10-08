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
using MoneyChest.Model.Enums;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(LimitHistory))]
    public class Limit : IHasId, IHasUserId
    {
        public Limit()
        {
            Categories = new List<LimitCategory>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(MaxSize.DescriptionLength)]
        public string Description { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateUntil { get; set; }

        public decimal Value { get; set; }

        public decimal SpentValue { get; set; }

        [StringLength(MaxSize.RemarkLength)]
        public string Remark { get; set; }

        public bool IncludeWithoutCategory { get; set; }

        public bool AllCategories { get; set; }

        public int CurrencyId { get; set; }

        [Required]
        public int UserId { get; set; }


        [ForeignKey(nameof(CurrencyId))]
        public virtual Currency Currency { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        
        public virtual ICollection<LimitCategory> Categories { get; set; }
    }
}
