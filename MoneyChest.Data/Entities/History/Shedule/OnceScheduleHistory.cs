using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities.History
{
    public class OnceScheduleHistory : ScheduleHistory
    {
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
    }
}
