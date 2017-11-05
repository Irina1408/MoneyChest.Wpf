using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Shared.Text
{
    public static class TextPack
    {
        private static ITextPack _textPack;

        public static string Dashboard => _textPack.Dashboard;
    }
}
