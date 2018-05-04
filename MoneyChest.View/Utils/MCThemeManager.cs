using MahApps.Metro;
using MaterialDesignThemes.Wpf;
using MoneyChest.Shared.MultiLang;
using MoneyChest.Shared.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MoneyChest.View.Utils
{
    public class ThemeColorData
    {
        public string Name { get; set; }
        public string Description => MultiLangResourceManager.Instance[MultiLangResourceName.ThemeColorName(Name)] ?? Name;
        public Brush ColorBrush { get; set; }
        public Brush BorderColorBrush { get; set; }
    }

    public class MCThemeManager
    {
        #region Singltone

        private static MCThemeManager currentThemeManager;
        public static MCThemeManager Instance => currentThemeManager ?? (currentThemeManager = new MCThemeManager());

        #endregion

        #region Private fields

        private List<ThemeColorData> _accentColors;
        private List<ThemeColorData> _themeColors;
        private string currentAccentColor = null;
        private string currentThemeColor = null;
        private PaletteHelper materialDesignPallet = new PaletteHelper();

        #endregion

        #region Public properties

        public const string DefaultAccentColor = "Violet";
        public const string DefaultThemeColor = "BaseLight";

        public List<ThemeColorData> AccentColors => _accentColors ?? (_accentColors = ThemeManager.Accents
                                            .Select(a => new ThemeColorData()
                                            {
                                                Name = a.Name,
                                                ColorBrush = a.Resources["AccentColorBrush"] as Brush,
                                                BorderColorBrush = a.Resources["AccentColorBrush"] as Brush
                                            }).ToList());

        public List<ThemeColorData> ThemeColors => _themeColors ?? (_themeColors = ThemeManager.AppThemes
                                           .Select(a => new ThemeColorData()
                                           {
                                               Name = a.Name,
                                               BorderColorBrush = a.Resources["BlackColorBrush"] as Brush,
                                               ColorBrush = a.Resources["WhiteColorBrush"] as Brush }
                                           ).ToList());

        #endregion

        #region Public methods

        public void SetTheme(string accentColor, string themeColor, bool updateSettings = false)
        {
            // check parameters
            if (string.IsNullOrEmpty(accentColor)) accentColor = DefaultThemeColor;
            if (string.IsNullOrEmpty(themeColor)) themeColor = DefaultThemeColor;
            // do not apply changes if parameters wasn't changed
            if (accentColor == currentAccentColor && themeColor == currentThemeColor) return;

            // chenga theme
            var theme = ThemeManager.GetAppTheme(themeColor) ?? ThemeManager.GetAppTheme(DefaultThemeColor);
            var accent = ThemeManager.GetAccent(accentColor) ?? ThemeManager.GetAccent(DefaultAccentColor);
            ThemeManager.ChangeAppStyle(Application.Current, accent, theme);

            // update matherial design accent color
            Application.Current.Resources["PrimaryHueMidBrush"] = Application.Current.Resources["AccentColorBrush"];
            Application.Current.Resources["PrimaryHueMidForegroundBrush"] = Application.Current.Resources["IdealForegroundColorBrush"];

            // update matherial design base color
            if (currentThemeColor != themeColor)
                materialDesignPallet.SetLightDark(themeColor.ToLower().Contains("dark"));

            // save current theme
            currentAccentColor = accentColor;
            currentThemeColor = themeColor;

            // update settings
            if(updateSettings)
            {
                AppSettings.Instance.LastAccentColor = currentAccentColor;
                AppSettings.Instance.LastThemeColor = currentThemeColor;
            }
        }

        public void SetAccentColor(string accentColor) => SetTheme(accentColor, currentThemeColor);
        public void SetThemeColor(string themeColor) => SetTheme(currentAccentColor, themeColor);

        #endregion
    }
}
