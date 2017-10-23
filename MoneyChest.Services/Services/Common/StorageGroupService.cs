﻿using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using MoneyChest.Model.Convert;
using MoneyChest.Model.Converters;
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
    public interface IStorageGroupService : IBaseIdManagableUserableListService<StorageGroupModel>
    {
    }

    public class StorageGroupService : BaseHistoricizedIdManageableUserableListService<StorageGroup, StorageGroupModel, StorageGroupConverter>, IStorageGroupService
    {
        public StorageGroupService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
