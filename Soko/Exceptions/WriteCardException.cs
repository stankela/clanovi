using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soko.Exceptions
{
    public class WriteCardException : ApplicationException
    {
        public WriteCardException()
        {
        }

        public WriteCardException(string message)
            : base(message)
        {
        }
    }
}
