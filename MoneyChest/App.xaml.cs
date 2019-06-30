using MoneyChest.Services;
using MoneyChest.Services.Execution;
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

            // dispose global service manager
            Exit += (sender, e) => Dispose();
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => Dispose();
        }

        private void Dispose()
        {
            MCTaskScheduler.Instance.End();
            ServiceManager.Dispose();
        }
    }
}
