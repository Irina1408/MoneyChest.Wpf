using MoneyChest.Data.Exceptions;
using MoneyChest.Model.Base;
using MoneyChest.Services.Services.Base;
using MoneyChest.Shared.MultiLang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MoneyChest.View.Utils
{
    public static class EntityViewHelper
    {
        public static bool ConfirmAndRemoveNamed<T>(IServiceBase<T> service, T entity, Action success = null)
            where T : class, IHasName
            => ConfirmAndRemove(service, new[] { entity }, EntityName<T>(), new[] { entity.Name }, success);

        public static bool ConfirmAndRemoveNamed<T>(IServiceBase<T> service, IEnumerable<T> entities, Action success = null)
            where T : class, IHasName
            => ConfirmAndRemove(service, entities, EntityName<T>(), entities.Select(x => x.Name).ToArray(), success);

        public static bool ConfirmAndRemove<T>(IServiceBase<T> service, T entity, Action success = null)
            where T : class, IHasDescription
            => ConfirmAndRemove(service, new[] { entity }, EntityName<T>(), new[] { entity.Description }, success);

        public static bool ConfirmAndRemove<T>(IServiceBase<T> service, IEnumerable<T> entities, Action success = null)
            where T : class, IHasDescription
            => ConfirmAndRemove(service, entities, EntityName<T>(), entities.Select(x => x.Description).ToArray(), success);

        public static bool ConfirmAndRemove<T>(IServiceBase<T> service, T entity, 
            string entityName, string description, Action success = null)
            where T : class
            => ConfirmAndRemove(service, new []{ entity }, entityName, new []{ description }, success);

        public static bool ConfirmAndRemove<T>(IServiceBase<T> service, IEnumerable<T> entities, string entityName,
            IEnumerable<string> descriptions, Action success = null)
            where T : class
            => ConfirmAndRemove(entities, (e) => service.Delete(e), entityName, descriptions, success);

        public static bool ConfirmAndRemove<T>(IEnumerable<T> entities, Action<IEnumerable<T>> deleteAction, string entityName,
            IEnumerable<string> descriptions, Action success = null)
            where T : class
        {
            var message = MultiLangResource.DeletionConfirmationMessage(typeof(T), descriptions);

            if (MessageBox.Show(message, MultiLangResourceManager.Instance[MultiLangResourceName.DeletionConfirmation],
                MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.Yes) == MessageBoxResult.Yes)
            {
                try
                {
                    // remove in database
                    deleteAction(entities);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(MultiLangResource.DeletionErrorMessage(typeof(T), descriptions),
                        MultiLangResourceManager.Instance[MultiLangResourceName.DeletionError], MessageBoxButton.OK, MessageBoxImage.Error);

                    return false;
                }

                // apply if success result
                success?.Invoke();
                return true;
            }

            return false;
        }

        private static string EntityName<T>() => nameof(T).Replace("Model", "");
    }
}
