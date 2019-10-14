using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MoneyChest.View.Utils
{
    public class WaitCursor : IDisposable
    {
        private Cursor prevCursor;

        public WaitCursor()
        {
            prevCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait;
        }

        public void Dispose()
        {
            Mouse.OverrideCursor = prevCursor;
        }
    }
}
