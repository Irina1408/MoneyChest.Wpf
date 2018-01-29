using MoneyChest.Model.Model;
using MoneyChest.ViewModel.Extensions;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Shared.MultiLang;

namespace MoneyChest.ViewModel.ViewModel
{
    public class SimpleEventViewModel : SimpleEventModel
    {
        public SimpleEventViewModel() : base()
        { }

        public SimpleEventViewModel(SimpleEventModel model) : base()
        {
            // copy all properties
            foreach(var property in typeof(SimpleEventModel).GetProperties())
            {
                if (!property.CanRead || !property.CanWrite) continue;
                property.SetValue(this, property.GetValue(model));
            }
        }

        public string ScheduleDetailed => Schedule?.DetailedSchedule(DateFrom, DateUntil);
        public string EventStateDetailed => EventState == EventState.Paused && PausedToDate.HasValue
            ? MultiLangResource.PausedToDate(PausedToDate.Value)
            : MultiLangResource.EnumItemDescription(nameof(Model.Enums.EventState), EventState.ToString());
    }

    public class MoneyTransferEventViewModel : MoneyTransferEventModel
    {
        public MoneyTransferEventViewModel() : base()
        { }

        public MoneyTransferEventViewModel(MoneyTransferEventModel model) : base()
        {
            // copy all properties
            foreach (var property in typeof(MoneyTransferEventModel).GetProperties())
            {
                if (!property.CanRead || !property.CanWrite) continue;
                property.SetValue(this, property.GetValue(model));
            }
        }

        public string ScheduleDetailed => Schedule?.DetailedSchedule(DateFrom, DateUntil);
        public string EventStateDetailed => EventState == EventState.Paused && PausedToDate.HasValue
            ? MultiLangResource.PausedToDate(PausedToDate.Value)
            : MultiLangResource.EnumItemDescription(nameof(Model.Enums.EventState), EventState.ToString());
    }

    public class RepayDebtEventViewModel : RepayDebtEventModel
    {
        public RepayDebtEventViewModel() : base()
        { }

        public RepayDebtEventViewModel(RepayDebtEventModel model) : base()
        {
            // copy all properties
            foreach (var property in typeof(RepayDebtEventModel).GetProperties())
            {
                if (!property.CanRead || !property.CanWrite) continue;
                property.SetValue(this, property.GetValue(model));
            }
        }

        public string ScheduleDetailed => Schedule?.DetailedSchedule(DateFrom, DateUntil);
        public string EventStateDetailed => EventState == EventState.Paused && PausedToDate.HasValue
            ? MultiLangResource.PausedToDate(PausedToDate.Value)
            : MultiLangResource.EnumItemDescription(nameof(Model.Enums.EventState), EventState.ToString());
    }
}
