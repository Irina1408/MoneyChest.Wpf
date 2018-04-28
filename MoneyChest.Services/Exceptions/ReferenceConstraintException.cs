using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Exceptions
{
    [Serializable]
    public sealed class ReferenceConstraintException : ConstraintException
    {
        const string defaultMessage = "A constraint reference has been detected while saving data. Please verify entries and try again.";

        public ReferenceConstraintException() : base(defaultMessage)
        { }

        public ReferenceConstraintException(DbUpdateException inner) : base(defaultMessage, inner)
        { }

        public ReferenceConstraintException(string message)
            : base(message)
        { }

        public ReferenceConstraintException(string message, DbUpdateException inner)
            : base(message, inner)
        { }
    }
}
