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
    public sealed class ViolationOfConstraintException : ConstraintException
    {
        public const string defaultMessage = "A constraint violation has been detected while saving data. Please verify entries and try again.";

        public ViolationOfConstraintException() : base(defaultMessage)
        { }

        public ViolationOfConstraintException(DbUpdateException inner) : base(defaultMessage, inner)
        { }

        public ViolationOfConstraintException(string message)
            : base(message)
        { }

        public ViolationOfConstraintException(string message, DbUpdateException inner)
            : base(message, inner)
        { }
    }
}
