using System;

namespace Soko.Exceptions
{
	/// <summary> This exception is used to mark business rule violations. </summary>
	[Serializable]
	public class BusinessException : ApplicationException
	{
        private Notification _notification;
        public Notification Notification
        {
            get { return _notification; }
        }

        //Expose a number of handy public constructors, 
        //that simply delegate to the base class.
		public BusinessException(){}
		public BusinessException(string message) : base(message){}
		public BusinessException(string message, Exception cause) : base(message, cause){}
		public BusinessException(Exception cause) : base("Business rule violation", cause){}
        
        public BusinessException(Notification notification) : base("Business rule violation")
        {
            _notification = notification;
        }
    }
}