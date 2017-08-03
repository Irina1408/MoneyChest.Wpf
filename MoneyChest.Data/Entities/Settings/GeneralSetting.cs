﻿using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities
{
    public class GeneralSetting
    {
        public GeneralSetting()
        {
            Language = Language.English;
            HideCoinBoxStorages = true;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Hide coin box accounts in every selection
        public bool HideCoinBoxStorages { get; set; }

        public Language Language { get; set; }

        public int DebtCategoryId { get; set; }

        // Money transfer comission category
        public int ComissionCategoryId { get; set; }


        [ForeignKey(nameof(DebtCategoryId))]
        public virtual Category DebtCategory { get; set; }
        
        [ForeignKey(nameof(ComissionCategoryId))]
        public virtual Category ComissionCategory { get; set; }

        [Required]
        public int UserId { get; set; }


        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}