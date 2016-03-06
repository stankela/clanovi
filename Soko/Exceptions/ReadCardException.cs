using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soko.Exceptions
{
    public class ReadCardException : ApplicationException
    {
        public ReadCardException()
        {
        }

        public ReadCardException(string message)
            : base(message)
        {
        }
    }
}
