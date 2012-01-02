using System;

namespace Soko.Exceptions
{
    [Serializable]
    public class InvalidPropertyException : Exception
    {
        private string invalidProperty;

        public string InvalidProperty
        {
            get { return invalidProperty; }
        }

        public InvalidPropertyException()
        {

        }

        public InvalidPropertyException(string message, string propertyName)
            : base(message)
        {
            this.invalidProperty = propertyName;
        }

        public InvalidPropertyException(string message, string propertyName,
            Exception inner)
            : base(message, inner)
        {
            this.invalidProperty = propertyName;
        }
    }
}
