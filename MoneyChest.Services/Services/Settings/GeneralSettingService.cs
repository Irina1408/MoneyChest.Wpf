﻿using MoneyChest.Data.Entities;
using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using System.Linq.Expressions;
using MoneyChest.Model.Model;
using MoneyChest.Services.Converters;
using System.Data.Entity;

namespace MoneyChest.Services.Services.Settings
{
    public interface IGeneralSettingService : IUserSettingsService<GeneralSettingModel>
    {
    }

    public class GeneralSettingService : UserSettingServiceBase<GeneralSetting, GeneralSettingModel, GeneralSettingConverter>, IGeneralSettingService
    {
        public GeneralSettingService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
