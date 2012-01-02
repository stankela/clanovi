using System;

namespace Soko.Domain
{
    public abstract class DomainObject
    {
		private Key key;
		public Key Key
		{
			get { return key; }
			set { key = value; }
		}
	
		public virtual void validate(Notification notification)
		{
			throw new Exception("Derived class should implement this method.");
		}
	}
}
