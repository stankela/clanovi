using System;

namespace Soko.Domain
{
    public abstract class DomainObject
    {
        private int id;
        public virtual int Id
        {
            get { return id; }
            set { id = value; }
        }

        private Key key;
        public virtual Key Key
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
