﻿using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Converters
{
    public class CalendarSettingsConverter : EntityModelConverterBase<CalendarSettings, CalendarSettingsModel>
    {
        private DataFilterConverter dataFilterConverter = new DataFilterConverter();
        private PeriodFilterConverter periodFilterConverter = new PeriodFilterConverter();

        protected override void FillEntity(CalendarSettings entity, CalendarSettingsModel model)
        {
            entity.ShowAllLimits = model.ShowAllLimits;
            entity.ShowSettings = model.ShowSettings;
            entity.ShowAllStorages = model.ShowAllStorages;
            entity.ShowAllTransactionsPerDay = model.ShowAllTransactionsPerDay;
            entity.MaxTransactionsCountPerDay = model.MaxTransactionsCountPerDay;
            entity.UserId = model.UserId;

            if (entity.PeriodFilter != null)
                periodFilterConverter.UpdateEntity(entity.PeriodFilter, model.PeriodFilter);

            if (entity.DataFilter != null)
                dataFilterConverter.UpdateEntity(entity.DataFilter, model.DataFilter);
        }

        protected override void FillModel(CalendarSettings entity, CalendarSettingsModel model)
        {
            model.ShowAllLimits = entity.ShowAllLimits;
            model.ShowSettings = entity.ShowSettings;
            model.ShowAllStorages = entity.ShowAllStorages;
            model.ShowAllTransactionsPerDay = entity.ShowAllTransactionsPerDay;
            model.MaxTransactionsCountPerDay = entity.MaxTransactionsCountPerDay;
            model.UserId = entity.UserId;

            if (entity.PeriodFilter != null)
                periodFilterConverter.UpdateModel(entity.PeriodFilter, model.PeriodFilter);

            if (entity.DataFilter != null)
                dataFilterConverter.UpdateModel(entity.DataFilter, model.DataFilter);
        }
    }
}
