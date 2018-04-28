using MoneyChest.Model.Base;
using MoneyChest.Model.Constants;
using MoneyChest.Model.Enums;
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
            Language = Language.English;
        }

        public int Id { get; set; }

        [StringLength(MaxSize.NameLength)]
        public string Name { get; set; }
        
        public string Password { get; set; }

        public DateTime LastUsageDate { get; set; }

        public DateTime? LastSynchronizationDate { get; set; }

        public Language Language { get; set; }

        public string ServerUserId { get; set; }
    }
}
