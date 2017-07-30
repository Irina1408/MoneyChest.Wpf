using MoneyChest.Data.Attributes;
using MoneyChest.Data.Entities.History;
using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Services.Base.History
{
    internal static class HistoryExtensions
    {
        private static Dictionary<Type, HistoricizedEntityDetails> EntityHistoricized = new Dictionary<Type, HistoricizedEntityDetails>();

        public static int UserId { get; set; }

        public static object CreateHistoryItem(object entity, ActionType actionType)
        {
            if (IsHistoricizedType(entity.GetType()))
            {
                HistoricizedEntityDetails details = null;
                if (!EntityHistoricized.TryGetValue(entity.GetType(), out details))
                    details = DefineEntityHistoricized(entity.GetType());

                return Fill(entity, actionType, details);
            }

            return null;
        }

        private static bool IsHistoricizedType(Type type)
        {
            return type.GetCustomAttributes(typeof(HistoricizedAttribute), true).Length > 0;
        }

        private static HistoricizedEntityDetails DefineEntityHistoricized(Type type)
        {
            var atr = type.GetCustomAttributes(typeof(HistoricizedAttribute), true)[0] as HistoricizedAttribute;

            if (!EntityHistoricized.ContainsKey(type))
            {
                var details = new HistoricizedEntityDetails() { HistoryType = atr.HistoricalType };
                var historyProperies = atr.HistoricalType.GetProperties();
                var entityProperies = type.GetProperties();

                foreach (var prop in historyProperies.Where(item => entityProperies.Any(e => e.Name == item.Name)))
                {
                    details.HistoryProperties.Add(prop, entityProperies.FirstOrDefault(item => item.Name == prop.Name));
                }
            }

            return EntityHistoricized[type];
        }

        private static object Fill(object entity, ActionType actionType, HistoricizedEntityDetails details)
        {
            try
            {
                var historyItem = Activator.CreateInstance(details.HistoryType);
                if(historyItem is IUserActionHistory)
                {
                    ((IUserActionHistory)historyItem).ActionDateTime = DateTime.Now;
                    ((IUserActionHistory)historyItem).ActionType = actionType;
                    ((IUserActionHistory)historyItem).UserId = UserId;
                }
                foreach(var prop in details.HistoryProperties)
                {
                    prop.Key.SetValue(historyItem, prop.Value.GetValue(entity));
                }
            }
            catch(Exception)
            { }

            return null;
        }

        private class HistoricizedEntityDetails
        {
            public HistoricizedEntityDetails()
            {
                HistoryProperties = new Dictionary<PropertyInfo, PropertyInfo>();
            }

            public Type HistoryType { get; set; }
            public Dictionary<PropertyInfo, PropertyInfo> HistoryProperties { get; set; }
        }
    }
}
