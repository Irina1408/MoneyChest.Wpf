using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using MoneyChest.Model.Extensions;
using MoneyChest.Data.Converters;
using MoneyChest.Model.Model;
using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Services
{
    public interface IStorageGroupService : IIdManagableUserableListServiceBase<StorageGroupModel>
    {
    }

    public class StorageGroupService : HistoricizedIdManageableUserableListServiceBase<StorageGroup, StorageGroupModel, StorageGroupConverter>, IStorageGroupService
    {
        public StorageGroupService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
