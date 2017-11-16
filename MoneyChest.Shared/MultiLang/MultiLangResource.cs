using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Shared.MultiLang
{
    public static class MultiLangResource
    {
        public static string DeletionConfirmationMessage(Type entityType, IEnumerable<string> items) =>
            DeletionConfirmationMessage(entityType.Name.Replace("Model", ""), items);

        public static string DeletionConfirmationMessage(string entityName, IEnumerable<string> items) =>
            string.Format(MultiLangResourceManager.Instance[MultiLangResourceName.DeletionConfirmationMessage],
                items.Count() == 1 
                ? MultiLangResourceManager.Instance[MultiLangResourceName.Singular(entityName)].ToLower()
                : MultiLangResourceManager.Instance[MultiLangResourceName.Plural(entityName)].ToLower(),
                string.Join(";", items));
    }
}
