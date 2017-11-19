using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Shared.MultiLang
{
    public static class MultiLangResourceName
    {
        #region General

        public static string DeletionConfirmation => "General_DeletionConfirmation";
        public static string DeletionConfirmationMessage => "General_DeletionConfirmationMessage";

        public static string SaveChangesConfirmation => "General_SaveChangesConfirmation";
        public static string SaveChangesConfirmationMessage => "General_SaveChangesConfirmationMessage";

        public static string SaveFailed => "General_SaveFailed";
        public static string SaveFailedMessage => "General_SaveFailedMessage";

        public static string None => "General_None";

        #endregion

        #region Complex

        public static string EnumName(Type enumType) => $"{enumType.Name}_Name";

        public static string Singular(Type entityType) => Singular(entityType.Name.Replace("Model", ""));
        public static string Plural(Type entityType) => Plural(entityType.Name.Replace("Model", ""));
        public static string New(Type entityType) => New(entityType.Name.Replace("Model", ""));

        public static string Singular(string entityName) => $"{entityName}_Singular";
        public static string Plural(string entityName) => $"{entityName}_Plural";
        public static string New(string entityName) => $"{entityName}_New";

        #endregion

        #region Login window

        public static string LogIn => "LoginWindow_Login";
        public static string Register => "LoginWindow_Register";
        public static string LoginFailed => "LoginWindow_LoginFailed";
        public static string LoginFailedMessage => "LoginWindow_LoginFailedMessage";
        public static string RegistrationFailed => "LoginWindow_RegistrationFailed";
        public static string RegistrationFailedMessage => "LoginWindow_RegistrationFailedMessage";

        #endregion

        #region Main menu

        public static string Dashboard => "MainManu_Dashboard";
        public static string Transactions => "MainManu_Transactions";
        public static string Calendar => "MainManu_Calendar";
        public static string Accounts => "MainManu_Accounts";
        public static string Planning => "MainManu_Planning";
        public static string Debts => "MainManu_Debts";
        public static string Reports => "MainManu_Reports";
        public static string Currencies => "MainManu_Currencies";
        public static string Categories => "MainManu_Categories";

        #endregion
    }
}
