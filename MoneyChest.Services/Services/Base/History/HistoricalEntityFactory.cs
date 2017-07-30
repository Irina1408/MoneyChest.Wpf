using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Services.Base.History
{
    internal class HistoricalEntityFactory
    {
        private readonly Dictionary<Type, Action<object, object>> _historicalItemDefinitions = new Dictionary<Type, Action<object, object>>();
        
        public void Define<T, THistory>(Action<T, THistory> action)
        {
            if (!_historicalItemDefinitions.ContainsKey(typeof(T)))
                _historicalItemDefinitions.Add(typeof(T), (item, historyItem) => action((T)item, (THistory)historyItem));
            else
                _historicalItemDefinitions[typeof(T)] = (item, historyItem) => action((T)item, (THistory)historyItem);
        }

        public void Fill<T, THistory>(T item, THistory historyItem)
        {
            try
            {
                Action<object, object> action;
                if (_historicalItemDefinitions.TryGetValue(typeof(T), out action))
                    action.Invoke(item, historyItem);
            }
            catch(Exception)
            {
            }
        }
    }
}
