using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Convert
{
    public static class ToReferenceViewConverter
    {
        public static CurrencyReference ToReferenceView(this Currency entity)
        {
            return new CurrencyReference { Id = entity.Id, Code = entity.Code };
        }

        public static CurrencyReference ToReferenceView(this CurrencyModel entity)
        {
            return new CurrencyReference { Id = entity.Id, Code = entity.Code };
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
