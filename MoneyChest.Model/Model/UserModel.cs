using MoneyChest.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class UserModel : IHasId
    {
        public UserModel()
        {
            LastUsageDate = DateTime.Now;
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
        
        public string Password { get; set; }

        public DateTime LastUsageDate { get; set; }

        public DateTime? LastSynchronizationDate { get; set; }

        public string ServerUserId { get; set; }
    }
}
