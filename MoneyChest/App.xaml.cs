using MoneyChest.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MoneyChest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() : base()
        {
            System.Windows.FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;

            // init global service manager
            ServiceManager.Initialize();

            // dispose global service manager
            Exit += (sender, e) => ServiceManager.Dispose();
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => ServiceManager.Dispose();
        }
    }
}
