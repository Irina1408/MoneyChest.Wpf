﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Entities;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;

namespace MoneyChest.Services.Services.Settings
{
    public class ReportSettingService : BaseService<ReportSetting>
    {
        public ReportSettingService(ApplicationDbContext context) : base(context)
        {
        }
    }
}