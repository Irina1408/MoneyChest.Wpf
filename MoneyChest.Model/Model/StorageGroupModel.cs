using MoneyChest.Model.Base;
using MoneyChest.Model.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class StorageGroupModel : IHasId, IHasUserId, IHasName, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }

        [StringLength(MaxSize.NameLength)]
        public string Name { get; set; }
        
        public int UserId { get; set; }
    }
}
