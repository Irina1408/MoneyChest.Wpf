using MoneyChest.Model.Base;
using MoneyChest.Model.Constants;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class CategoryModel : IHasId, IHasUserId, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public CategoryModel()
        {
            IsActive = true;
        }

        public int Id { get; set; }

        [StringLength(MaxSize.NameLength)]
        public string Name { get; set; }

        public RecordType? RecordType { get; set; }

        public bool IsActive { get; set; }

        [StringLength(MaxSize.RemarkLength)]
        public string Remark { get; set; }

        public int? ParentCategoryId { get; set; }
        public int UserId { get; set; }

        
        public bool RecordTypeEnabled
        {
            get => RecordType.HasValue;
            set => RecordType = value ? (RecordType?)Enums.RecordType.Expense : null;
        }
    }
}
