using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities
{
    public class SettingCategorized
    {
        public SettingCategorized()
        {
            AllCategories = true;

            Categories = new List<Category>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public bool AllCategories { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
