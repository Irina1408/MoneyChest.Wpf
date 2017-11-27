using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Extensions
{
    public static class ToReferenceViewExtensions
    {
        public static CurrencyReference ToReferenceView(this Currency entity)
        {
            return new CurrencyReference
            {
                Id = entity.Id,
                Name = entity.Name,
                Symbol = entity.Symbol,
                SymbolAlignment = entity.CurrencySymbolAlignment
            };
        }

        public static StorageGroupReference ToReferenceView(this StorageGroup entity)
        {
            return new StorageGroupReference { Id = entity.Id, Name = entity.Name };
        }

        public static StorageReference ToReferenceView(this Storage entity)
        {
            return new StorageReference
            {
                Id = entity.Id,
                Name = entity.Name,
                StorageGroupId = entity.StorageGroupId,
                CurrencyId = entity.CurrencyId
            };
        }

        public static CategoryReference ToReferenceView(this Category entity)
        {
            return new CategoryReference { Id = entity.Id, Name = entity.Name };
        }

        public static DebtReference ToReferenceView(this Debt entity)
        {
            return new DebtReference { Id = entity.Id, Description = entity.Description, DebtType = entity.DebtType };
        }

        public static MoneyTransferReference ToReferenceView(this MoneyTransfer entity)
        {
            return new MoneyTransferReference { Id = entity.Id, Description = entity.Description };
        }
    }
}
