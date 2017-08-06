using MoneyChest.Data.Entities.History;
using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace MoneyChest.Tests.Services
{
    public class HistoricizedServiceTestBase : IntegrationTestBase
    {
        protected void CheckLastChangeHistory<T, THistory>(ActionType actionType, T entity)
            where T : class
            where THistory : class, IUserActionHistory, new()
        {
            var historyItem = App.Db.Set<THistory>()
                .OrderByDescending(item => item.ActionDateTime)
                .FirstOrDefault(item => item.ActionType == actionType && item.UserId == user.Id);

            var historyProperies = typeof(THistory).GetProperties();
            var entityProperies = typeof(T).GetProperties();

            foreach (var prop in historyProperies
                .Where(item => item.CanWrite && entityProperies.Any(e => e.Name == item.Name && e.CanRead)))
            {
                var entityProp = entityProperies.FirstOrDefault(item => item.Name == prop.Name);

                prop.GetValue(historyItem).ShouldBeEquivalentTo(entityProp.GetValue(entity));
            }
        }
    }
}
