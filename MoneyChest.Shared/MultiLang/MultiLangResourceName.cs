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
        public static string DeletionError => "General_DeletionError";
        public static string DeletionErrorMessageOne => "General_DeletionErrorMessageOne";
        public static string DeletionErrorMessagePlural => "General_DeletionErrorMessagePlural";

        public static string SaveChangesConfirmation => "General_SaveChangesConfirmation";
        public static string SaveChangesConfirmationMessage => "General_SaveChangesConfirmationMessage";

        public static string SaveFailed => "General_SaveFailed";
        public static string SaveFailedMessage => "General_SaveFailedMessage";

        public static string Today => "General_Today";
        public static string None => "General_None";
        public static string All => "General_All";
        public static string SuspendUntil => "Event_SuspendUntil";
        public static string Amount => "General_Amount";

        #endregion

        #region Complex

        //public static string EnumName(Type enumType) => $"{enumType.Name}_EnumName";
        //public static string EnumValue(Type enumType, string name) => $"{enumType.Name}_{name}";

        public static string Singular(Type entityType) => Singular(entityType.Name.Replace("ViewModel", "").Replace("Model", ""));
        public static string Plural(Type entityType) => Plural(entityType.Name.Replace("ViewModel", "").Replace("Model", ""));
        public static string New(Type entityType) => New(entityType.Name.Replace("ViewModel", "").Replace("Model", ""));

        public static string Singular(string entityName) => $"{entityName}_Singular";
        public static string Plural(string entityName) => $"{entityName}_Plural";
        public static string New(string entityName) => $"{entityName}_New";

        public static string MainMenuPageCaption(string page) => $"MainManu_{page}";
        
        public static string ThemeColorName(string color) => $"ThemeColor_{color}";

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
        public static string Storages => "MainManu_Storages";
        public static string Planning => "MainManu_Planning";
        public static string Debts => "MainManu_Debts";
        public static string Reports => "MainManu_Reports";
        public static string Currencies => "MainManu_Currencies";
        public static string Categories => "MainManu_Categories";

        #endregion

        #region Schedule

        public static string EveryDay => "Schedule_EveryDay";
        public static string EveryNumberDays => "Schedule_EveryNumberDays";
        public static string EveryWeek => "Schedule_Every_ForWeek";
        public static string EveryNumberWeeks => "Schedule_EveryNumberWeeks";
        public static string EveryDayNumberWeeks => "Schedule_EveryDayNumberWeeks";
        public static string EverySpecialMonth => "Schedule_Every_ForMonth";
        public static string EveryMonth => "Schedule_Every_ForEveryMonth";
        public static string EverySpecialMonthLastDay => "Schedule_EveryLastDay_ForMonth";
        public static string EveryMonthLastDay => "Schedule_EveryLastDay_ForEveryMonth";
        public static string FromForDate => "Schedule_From_ForDate";
        public static string UntilForDate => "Schedule_Until_ForDate";
        public static string InDayOfWeek(DayOfWeek dayOfWeek) => $"{nameof(DayOfWeek)}_{dayOfWeek.ToString()}_In";
        public static string PausedToDate => "Schedule_PausedToDate";
        public static string LastForDayOfMonth => "Schedule_DayOfMonth_Last";

        #endregion
    }
}
