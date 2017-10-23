using MoneyChest.Data.Entities.Base;
using System;
using System.Collections.Generic;
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
        
        public string Name { get; set; }
        
        public string Password { get; set; }

        public string Email { get; set; }

        public DateTime LastUsageDate { get; set; }

        public DateTime? LastSynchronizationDate { get; set; }

        public string ServerUserId { get; set; }
    }
}
