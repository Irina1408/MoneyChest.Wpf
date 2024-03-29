﻿using MoneyChest.Data.Attributes;
using MoneyChest.Model.Base;
using MoneyChest.Data.Entities.History;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Model.Constants;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(StorageGroupHistory))]
    public class StorageGroup : IHasId, IHasUserId
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(MaxSize.NameLength)]
        public string Name { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        public virtual ICollection<Storage> Storages { get; set; }
        public virtual ICollection<CalendarSettings> CalendarSettings { get; set; }
    }
}
