using MoneyChest.Shared.MultiLang;
using MoneyChest.View.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MoneyChest.View.Pages
{
    public abstract class PageBase : UserControl, IPage
    {
        #region Private fields

        private bool _initializationComplete = false;

        #endregion

        #region Initialization

        public PageBase() : base()
        {
        }

        #endregion

        #region IPage implementation

        // General view options
        public string Label => 
            MultiLangResourceManager.Instance[MultiLangResourceName.MainMenuPageCaption(this.GetType().Name.Replace("Page", ""))];
        public FrameworkElement Icon => PageHelper.GetPageIcon(this);
        public int Order => PageHelper.GetPageOrder(this);
        public FrameworkElement View => this;
        public virtual bool ShowTopBorder => false;
        public virtual bool IsOptionsPage => false;

        // Data management
        public event EventHandler DataChanged;
        public bool RequiresReload { get; set; } = true;
        public virtual void Reload()
        {
            // complete initialization once
            if (!_initializationComplete)
            {
                InitializationComplete();
                _initializationComplete = true;
            }

            // mark as reloaded
            RequiresReload = false;
        }

        #endregion

        #region Protected methods

        protected virtual void InitializationComplete() { }

        protected void NotifyDataChanged()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
