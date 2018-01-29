using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class DebtReference
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DebtType DebtType { get; set; }
        public int CurrencyId { get; set; }
    }
}
