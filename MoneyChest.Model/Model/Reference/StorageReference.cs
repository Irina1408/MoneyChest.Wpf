using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class StorageReference
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StorageGroupId { get; set; }
        public int CurrencyId { get; set; }
        public bool IsVisible { get; set; }
    }
}
