using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Exceptions
{
    public class WriteHistoryException : Exception
    {
        public WriteHistoryException() 
            :base("History updating error")
        { }

        public WriteHistoryException(string message) 
            : base(message)
        { }
    }
}
