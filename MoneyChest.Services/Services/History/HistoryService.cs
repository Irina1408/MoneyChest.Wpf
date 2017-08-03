using MoneyChest.Data.Attributes;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities.History;
using MoneyChest.Data.Enums;
using MoneyChest.Services.Exceptions;
using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Services.History
{
    internal interface IHistoryService : IBaseService
    {
        void WriteHistory(object entity, ActionType actionType, int userId);
        void WriteHistory<T, THistory>(T entity, ActionType actionType, int userId, Action<THistory> overrides = null)
        where T : class
        where THistory : class, IUserActionHistory, new();
    }

    internal class HistoryService : BaseService, IHistoryService
    {
        #region Private fields

        private List<HistoricizedEntityHelper> _historicizedEntityHelpers;
        private List<KeyValuePair<object, IUserActionHistory>> _history;

        #endregion

        #region Initialization

        public HistoryService(ApplicationDbContext context) : base(context)
        {
            _historicizedEntityHelpers = new List<HistoricizedEntityHelper>();
            _history = new List<KeyValuePair<object, IUserActionHistory>>();
        }

        #endregion

        #region Public methods

        public void WriteHistory(object entity, ActionType actionType, int userId)
        {
            // try find history helper
            var helper = _historicizedEntityHelpers.FirstOrDefault(item => item.EntityType == entity.GetType());
            if(helper == null)
            {
                // get history type
                var historyType = GetHistoryType(entity.GetType());
                // check history type
                if (historyType.BaseType != typeof(IUserActionHistory))
                    throw new WriteHistoryException("Incorrect history type");

                // prepare entity history hepler
                _historicizedEntityHelpers.Add(new HistoricizedEntityHelper(entity.GetType(), historyType));
            }

            //var historySet = _context.Set(helper.HistoryType);
            var historyItem = Activator.CreateInstance(helper.HistoryType) as IUserActionHistory;

            // fill historical entity
            historyItem.ActionDateTime = DateTime.Now;
            historyItem.ActionType = actionType;
            historyItem.UserId = userId;
            if(actionType != ActionType.Add)
                helper.FillHistoryItem(entity, historyItem);
            // write to the history
            _history.Add(new KeyValuePair<object, IUserActionHistory>(entity, historyItem));

            //helper.FillHistoryItem(entity, historyItem);

            //historySet.Add(historyItem);
        }

        public void WriteHistory<T, THistory>(T entity, ActionType actionType, int userId, Action<THistory> overrides = null)
            where T : class
            where THistory : class, IUserActionHistory, new()
        {
            //var historySet = _context.Set<THistory>();
            var historyItem = new THistory()
            {
                ActionDateTime = DateTime.Now,
                ActionType = actionType,
                UserId = userId
            };

            // fill historical entity
            var helper = _historicizedEntityHelpers
                .FirstOrDefault(item => item.EntityType == typeof(T) && item.HistoryType == typeof(THistory));
            if (helper == null)
                _historicizedEntityHelpers.Add(new HistoricizedEntityHelper(typeof(T), typeof(THistory)));
            if (actionType != ActionType.Add)
                helper.FillHistoryItem(entity, historyItem);
            overrides?.Invoke(historyItem);
            // write to the history
            _history.Add(new KeyValuePair<object, IUserActionHistory>(entity, historyItem));

            //historySet.Add(historyItem);
        }

        #endregion

        #region Overrides

        public override void SaveChanges()
        {
            foreach(var historyItem in _history)
            {
                var historySet = _context.Set(historyItem.Key.GetType());
                if (historyItem.Value.ActionType == ActionType.Add)
                {
                    var helper = _historicizedEntityHelpers.FirstOrDefault(item => item.EntityType == historyItem.Key.GetType());
                    helper.FillHistoryItem(historyItem.Key, historyItem);
                }
                historySet.Add(historyItem);
            }
            base.SaveChanges();
        }

        public override async Task SaveChangesAsync()
        {
            foreach (var historyItem in _history)
            {
                var historySet = _context.Set(historyItem.Key.GetType());
                if (historyItem.Value.ActionType == ActionType.Add)
                {
                    var helper = _historicizedEntityHelpers.FirstOrDefault(item => item.EntityType == historyItem.Key.GetType());
                    helper.FillHistoryItem(historyItem.Key, historyItem);
                }
                historySet.Add(historyItem);
            }
            await base.SaveChangesAsync();
        }

        #endregion

        #region Private methods and classes

        private Type GetHistoryType(Type type)
        {
            var historyTypeAttribute = type.GetCustomAttributes(typeof(HistoricizedAttribute), true);
            if (historyTypeAttribute.Length == 0) throw new WriteHistoryException("Can't find history type");
            return ((HistoricizedAttribute)historyTypeAttribute[0]).HistoricalType;
        }

        private class HistoricizedEntityHelper
        {
            private List<HistoryPropertyDetails> _historyProperties;

            public HistoricizedEntityHelper(Type entityType, Type historyType)
            {
                _historyProperties = new List<HistoryPropertyDetails>();
                EntityType = entityType;
                HistoryType = historyType;
                
                FillHistoryProperties();
            }

            public Type EntityType { get; }
            public Type HistoryType { get; }

            public void FillHistoryItem(object entity, object historyEntity)
            {
                // check types
                if (entity.GetType() != EntityType || historyEntity.GetType() != HistoryType)
                    throw new WriteHistoryException("Entity type or history type is not the same as expected");

                // fill properties
                foreach (var prop in _historyProperties)
                    prop.HistoryProperty.SetValue(historyEntity, prop.EntityProperty.GetValue(entity));
            }

            private void FillHistoryProperties()
            {
                var historyProperies = HistoryType.GetProperties();
                var entityProperies = EntityType.GetProperties();

                foreach (var prop in historyProperies
                    .Where(item => item.CanWrite && entityProperies.Any(e => e.Name == item.Name && e.CanRead)))
                {
                    _historyProperties.Add(new HistoryPropertyDetails()
                    {
                        EntityProperty = entityProperies.FirstOrDefault(item => item.Name == prop.Name),
                        HistoryProperty = prop
                    });
                }
            }

            private class HistoryPropertyDetails
            {
                public PropertyInfo EntityProperty { get; set; }
                public PropertyInfo HistoryProperty { get; set; }
            }
        }

        #endregion
    }
}
