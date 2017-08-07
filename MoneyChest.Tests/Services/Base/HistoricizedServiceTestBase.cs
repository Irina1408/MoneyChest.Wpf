using MoneyChest.Data.Entities.History;
using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MoneyChest.Services.Services.Base;

namespace MoneyChest.Tests.Services
{
    public abstract class HistoricizedServiceTestBase<T, TService, THistory> : UserableEntityServiceTestBase<T, TService>
        where T : class, new()
        where TService : BaseHistoricizedService<T>
        where THistory : class, IUserActionHistory, new()
    {
        #region Overrides

        protected override void OnEntityAdded(T entity) => CheckLastChangeHistory(ActionType.Add, entity);
        protected override void OnEntityUpdated(T entity) => CheckLastChangeHistory(ActionType.Update, entity);
        protected override void OnEntityRemoved(T entity) => CheckLastChangeHistory(ActionType.Delete, entity);

        #endregion

        #region Protected methods

        protected void CheckLastChangeHistory(ActionType actionType, T entity)
        {
            int userId = GetUserId(entity);
            var historyItem = App.Db.Set<THistory>()
                .OrderByDescending(item => item.ActionDateTime)
                .FirstOrDefault(item => item.ActionType == actionType && item.UserId == userId);

            historyItem.Should().NotBeNull();
            
            var historyProperies = typeof(THistory).GetProperties();
            var entityProperies = typeof(T).GetProperties();

            foreach (var prop in historyProperies
                .Where(item => item.CanWrite
                    && (item.PropertyType == typeof(string) || !item.PropertyType.IsClass)
                    && entityProperies.Any(e => e.Name == item.Name && e.CanRead)))
            {
                var entityProp = entityProperies.FirstOrDefault(item => item.Name == prop.Name);

                if(actionType != ActionType.Delete || Nullable.GetUnderlyingType(entityProp.PropertyType) == null)
                    prop.GetValue(historyItem).ShouldBeEquivalentTo(entityProp.GetValue(entity));
            }
        }

        #endregion
    }
}
