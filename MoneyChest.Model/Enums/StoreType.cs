using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Enums
{
    [Description("Store type")]
    public enum StoreType
    {
        [Description("Coin box (no spend)")]
        CoinBox = 0,

        [Description("Wallet (spend)")]
        Wallet = 1
    }
}
